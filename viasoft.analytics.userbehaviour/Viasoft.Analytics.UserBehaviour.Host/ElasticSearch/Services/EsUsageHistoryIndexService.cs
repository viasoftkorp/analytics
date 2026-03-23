using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nest;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.DTOs.LicenseUsageHistoryIndex;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Messages;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.PushNotifications.Abstractions.Notification;

namespace Viasoft.Analytics.UserBehaviour.Host.ElasticSearch.Services
{
    public class EsUsageHistoryIndexService : IEsUsageHistoryIndexService, ITransientDependency
    {
        private readonly Core.DDD.Repositories.IRepository<LicenseUsageHistory> _licenseUsageHistories;
        private readonly Core.DDD.Repositories.IRepository<LicenseUsageHistoryIndex> _licenseUsageHistoryIndex;
        private readonly IElasticClient _elasticClient;
        private readonly IConfiguration _configuration;
        private readonly IPushNotification _notifications;
        private readonly ICurrentTenant _currentTenant;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceBus _serviceBus;
        private readonly IMemoryCache _memoryCache;

        public EsUsageHistoryIndexService(Core.DDD.Repositories.IRepository<LicenseUsageHistory> licenseUsageHistories, 
            Core.DDD.Repositories.IRepository<LicenseUsageHistoryIndex> licenseUsageHistoryIndex, IElasticClient elasticClient, IPushNotification notifications, 
            IConfiguration configuration, ICurrentTenant currentTenant, IUnitOfWork unitOfWork, IServiceBus serviceBus, IMemoryCache memoryCache)
        {
            _licenseUsageHistories = licenseUsageHistories;
            _licenseUsageHistoryIndex = licenseUsageHistoryIndex;
            _elasticClient = elasticClient;
            _notifications = notifications;
            _configuration = configuration;
            _currentTenant = currentTenant;
            _unitOfWork = unitOfWork;
            _serviceBus = serviceBus;
            _memoryCache = memoryCache;
        }

        public async Task<bool> HasIndex()
        {
            return await _licenseUsageHistoryIndex.Where(f => f.TenantId == _currentTenant.Id).AnyAsync();
        }

        public async Task<LicenseUsageHistoryIndex> GetHistoryIndex(Guid? customTenantId = null)
        {
            var tenant = customTenantId ?? _currentTenant.Id;
            return await _licenseUsageHistoryIndex.FirstOrDefaultAsync(f => f.TenantId == tenant);
        }

        public async Task<bool> SeedHistories()
        {
            var hasData = await HasIndex();
            if (hasData)
                return false;
            LicenseUsageHistoryIndex currentIndex;
            using (_unitOfWork.Begin())
            {
                currentIndex = await SetIndexAsDuringReindexProcess();
                await _unitOfWork.CompleteAsync();
            }

            await DeleteAllHistories();
            var indexingPossibleError = await IndexAllHistories();

            if (string.IsNullOrEmpty(indexingPossibleError))
            {
                await MarkHistoricIndexAsCompleted(currentIndex);
            }
            else
            {
                await MarkHistoricIndexAsFailed(currentIndex, indexingPossibleError);
            }
            
            await EmitUsageHistoryIndexUpdate(currentIndex);
            
            return true;
        }
        
        public async Task Reindex(Guid reindexIdentifier, int page)
        {
            var currentIndex = await GetHistoryIndex();
            if (currentIndex != null && reindexIdentifier != currentIndex.ReindexIdentifier && currentIndex.IsIndexing)
            {
                return;
            }
            using (_unitOfWork.Begin())
            {
                currentIndex = await SetIndexAsDuringReindexProcess(currentIndex, reindexIdentifier);
                await _unitOfWork.CompleteAsync();
            }

            try
            {
                if (page == 0)
                {
                    await DeleteAllHistories();
                }
            }
            catch (Exception e)
            {
                currentIndex.ReindexIdentifier = null;
                currentIndex.IsIndexing = false;
                currentIndex.HasEsFailedSinceLastReindex = true;
                currentIndex.FailureStackTrace = e.StackTrace?[..Math.Min(e.StackTrace.Length, 8000)] ?? e.Message[..Math.Min(e.Message.Length, 8000)];
                await _licenseUsageHistoryIndex.UpdateAsync(currentIndex, true);
                throw;
            }

            var (currentPage, stackTrace) = await IndexFromPage(page);
            var finished = currentPage == page;
            var hasFailed = !string.IsNullOrEmpty(stackTrace);

            if (hasFailed)
            {
                await MarkHistoricIndexAsFailed(currentIndex, stackTrace);
            }
            else
            {
                if (finished)
                {
                    await MarkHistoricIndexAsCompleted(currentIndex);
                }
                else
                {
                    await _serviceBus.SendLocal(new EsReindexCommand
                    {
                        MessageIdentifier = reindexIdentifier,
                        CurrentPage = currentPage
                    });
                }
            }

            //se terminou ou falhou, manda notificação pro frontend
            if (hasFailed || finished)
            {
                await EmitUsageHistoryIndexUpdate(currentIndex);
            }
        }

        public async Task CreateHistoryIndex(Guid? customTenantId = null)
        {
            var tenantId = customTenantId ?? _currentTenant.Id;
            var key = $"ElasticSearchCreatedIndexes_{tenantId}";
            
            // para não ficar verificando se o índice existe toda hora, fazemos um cache em memória
            if (!_memoryCache.TryGetValue(key, out _))
            {
                var index = await _elasticClient.Indices.ExistsAsync(ElasticSearchConsts.GetIndexName(tenantId)); 
                if (!index.Exists)
                {
                    await _elasticClient.LowLevel.Indices.CreateAsync<CreateResponse>(
                        ElasticSearchConsts.GetIndexName(tenantId),
                        ElasticSearchConsts.UsageHistory.IndexDefinition
                    );

                    _memoryCache.Set(key, 0, TimeSpan.FromHours(8));
                }
            }
        }

        private async Task DeleteAllHistories()
        {
            var existsResponse = await _elasticClient.Indices.ExistsAsync(ElasticSearchConsts.GetIndexName(_currentTenant.Id));
            if (!existsResponse.IsValid || existsResponse.ServerError != null)
            {
                if (existsResponse.ApiCall?.OriginalException != null)
                {
                    throw existsResponse.ApiCall.OriginalException;
                }

                throw new Exception("Error during checking if index exists");
            }
            if (!existsResponse.Exists)
            {
                await CreateHistoryIndex();
                return;
            }
            
            var key = $"ElasticSearchCreatedIndexes_{_currentTenant.Id}";
            _memoryCache.Remove(key);
            
            var response = await _elasticClient.Indices.DeleteAsync(ElasticSearchConsts.GetIndexName(_currentTenant.Id));
            if (response.ServerError != null || response.IsValid == false || !response.Acknowledged)
            {
                if(response.ApiCall?.OriginalException != null)
                    throw response.ApiCall.OriginalException;
                throw new Exception("Error during delete all histories process");
            }
            await CreateHistoryIndex();
        }

        private async Task<LicenseUsageHistoryIndex> SetIndexAsDuringReindexProcess(LicenseUsageHistoryIndex currentIndex = null, Guid? reindexIdentifier = null)
        {
            if (currentIndex == null)
            {
                currentIndex = new LicenseUsageHistoryIndex
                {
                    IsIndexing = true,
                    ReindexIdentifier = reindexIdentifier,
                    TenantId = _currentTenant.Id
                };
                return await _licenseUsageHistoryIndex.InsertAsync(currentIndex);
            }
            if(!reindexIdentifier.HasValue || reindexIdentifier.Value == Guid.Empty)
                throw new ArgumentException(nameof(reindexIdentifier));

            currentIndex.IsIndexing = true;
            currentIndex.ReindexIdentifier = reindexIdentifier;
            return await _licenseUsageHistoryIndex.UpdateAsync(currentIndex);
        }

        private async Task<(int, string)> IndexFromPage(int page)
        {
            GetIndexConfiguration(out _, out var itemsToInsertPerRound);
            var indexName = ElasticSearchConsts.GetIndexName(_currentTenant.Id);
            
            var itemsToAdd = await _licenseUsageHistories.AsNoTracking()
                .Skip(page)
                .Take(itemsToInsertPerRound)
                .ToListAsync();
            
            var response = await _elasticClient.BulkAsync(s => s
                .Index(indexName)
                .IndexMany(itemsToAdd));
            
            var hasFailed = response.Errors || !response.IsValid;

            if (hasFailed)
            {
                // Returning the same page value, indicates that the process is finished. In this case with a error
                return (page, LogElasticSearchIndexError(response));
            }
            
            if (itemsToAdd.Count < itemsToInsertPerRound)
            {
                // Returning the same page value, indicates that the process is finished
                return (page, null);
            }
            
            // Still unfinished
            return (page + itemsToInsertPerRound, null);
        }

        private async Task<string> IndexAllHistories()
        {
            GetIndexConfiguration(out var itemsToGetPerRound, out var itemsToInsertPerRound);

            var indexName = ElasticSearchConsts.GetIndexName(_currentTenant.Id); // Here to avoid tenant recalculation
            var hasMoreItems = true;
            var counter = 0;
            do
            {
                var itemsToAdd = await _licenseUsageHistories.AsNoTracking()
                    .Skip(itemsToGetPerRound * counter)
                    .Take(itemsToGetPerRound)
                    .ToListAsync();
                
                for (var j = 0; j < itemsToAdd.Count; j += itemsToInsertPerRound)
                {
                    var itemsToInsert = itemsToAdd.Skip(j).Take(itemsToInsertPerRound).ToList();
                    var response = await _elasticClient.BulkAsync(s => s
                        .Index(indexName)
                        .IndexMany(itemsToInsert)
                    );
                    var hasFailed = response.Errors || !response.IsValid;
                    if (hasFailed)
                    {
                        return LogElasticSearchIndexError(response);
                    }
                }
                
                counter++;
                hasMoreItems = itemsToAdd.Count == itemsToGetPerRound;
                
            } while (hasMoreItems);
            
            return null;
        }

        private async Task MarkHistoricIndexAsFailed(LicenseUsageHistoryIndex currentIndex, string failureStackTrace = null)
        {
            currentIndex.HasEsFailedSinceLastReindex = true;
            currentIndex.LastModificationTime = DateTime.UtcNow;
            currentIndex.FailureStackTrace = failureStackTrace![..Math.Min(failureStackTrace.Length, 8000)];
            currentIndex.ReindexIdentifier = null;
            currentIndex.IsIndexing = false;

            await _licenseUsageHistoryIndex.UpdateAsync(currentIndex, true);
        } 

        private async Task MarkHistoricIndexAsCompleted(LicenseUsageHistoryIndex currentIndex)
        {
            currentIndex.HasEsFailedSinceLastReindex = false;
            currentIndex.LastModificationTime = DateTime.UtcNow;
            currentIndex.FailureStackTrace = null;
            currentIndex.ReindexIdentifier = null;
            currentIndex.IsIndexing = false;
            await _licenseUsageHistoryIndex.UpdateAsync(currentIndex, true);
        }

        private async Task EmitUsageHistoryIndexUpdate(LicenseUsageHistoryIndex currentIndex)
        {
            await _notifications.SendUpdateAsync(new LicenseUsageHistoryIndexUpdated
            {
                IsIndexing = currentIndex.IsIndexing,
                LastModificationTime = currentIndex.LastModificationTime,
                HasEsFailedSinceLastReindex = currentIndex.HasEsFailedSinceLastReindex
            });
        }

        private void GetIndexConfiguration(out int itemsToGetPerRound, out int itemsToInsertPerRound)
        {
            var itemsToGetPerRoundAsString = _configuration["UserBehaviourEs:ItemsToGetPerRound"];
            itemsToGetPerRound = !string.IsNullOrEmpty(itemsToGetPerRoundAsString)
                ? Convert.ToInt32(itemsToGetPerRoundAsString)
                : ElasticSearchConsts.UsageHistory.DefaultItemsToGetPerRound;
            
            var itemsToInsertPerRoundAsString = _configuration["UserBehaviourEs:ItemsToInsertPerRound"];
            itemsToInsertPerRound = !string.IsNullOrEmpty(itemsToInsertPerRoundAsString)
                ? Convert.ToInt32(itemsToInsertPerRoundAsString)
                : ElasticSearchConsts.UsageHistory.DefaultItemsToInsertPerRound;
        }

        private static string LogElasticSearchIndexError(BulkResponse response)
        {
            var stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.Append("Elastic Search error during INDEX");
                if (response.ItemsWithErrors != null && response.ItemsWithErrors.Any())
                {
                    foreach (var itemWithErrors in response.ItemsWithErrors)
                    {
                        stringBuilder.Append($"\t Failure Id: {itemWithErrors.Id}");
                        stringBuilder.Append($"\t\t Index: {itemWithErrors.Index}");
                        stringBuilder.Append($"\t\t Type: {itemWithErrors.Type}");
                        stringBuilder.Append($"\t\t Status: {itemWithErrors.Status}");
                        stringBuilder.Append($"\t\t Operation: {itemWithErrors.Operation}");
                        stringBuilder.Append($"\t\t Error: {itemWithErrors.Error}");
                        stringBuilder.Append($"\t\t Result: {itemWithErrors.Result}");
                    }
                    stringBuilder.Append("\n\n");
                }
                
                stringBuilder.Append("\n Additional Information:");
                stringBuilder.Append($"\t ServerError: {response.ServerError}");
                stringBuilder.Append($"\t OriginalException: {response.OriginalException}");
                stringBuilder.Append($"\t Took: {response.Took}");
                stringBuilder.Append($"\t HttpMethod: {response.ApiCall.HttpMethod}");
                stringBuilder.Append($"\t Uri: {response.ApiCall.Uri}");
                stringBuilder.Append($"\t DebugInformation: {response.DebugInformation}");
            }
            catch (Exception e)
            {
                stringBuilder.Append("Error trying to log Elastic Search errors for INDEX");
                stringBuilder.Append(e);
            }

            return stringBuilder.ToString();
        }
    }
}