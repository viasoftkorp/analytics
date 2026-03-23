using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Nest;
using Viasoft.Analytics.UserBehaviour.Domain.Consts;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service
{
    public class LicenseUsageHistoryService: ILicenseUsageHistoryService, ITransientDependency
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IElasticClient _elasticClient;
        private readonly ICurrentTenant _currentTenant;

        public LicenseUsageHistoryService(IMapper mapper, IDateTimeProvider dateTimeProvider, IElasticClient elasticClient, ICurrentTenant currentTenant)
        {
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _elasticClient = elasticClient;
            _currentTenant = currentTenant;
        }

        public async Task<PagedResultDto<LicenseUsageHistoryOutput>> GetAll(GetAllLicenseUsageHistory input)
        {
            var searchQuery = new SearchDescriptor<Entities.LicenseUsageHistory>()
                .Index(ElasticSearchConsts.GetIndexName(_currentTenant.Id))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            const string scoreFieldName = "_score";
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                if(!input.Sorting.Contains(" "))
                    throw new Exception($"Invalid value for: {nameof(input.Sorting)}");
                var splittedSorting = input.Sorting.Split(" ");
                var fieldToSort = GetSortingField(splittedSorting[0]);
                searchQuery = splittedSorting[1] == "desc"
                    ? searchQuery.Sort(s => s.Descending(scoreFieldName).Descending(fieldToSort))
                    : searchQuery.Sort(s => s.Descending(scoreFieldName).Ascending(fieldToSort));
            }
            else
            {
                var userFieldName = ElasticSearchUtils.GetEsFieldRaw(nameof(Entities.LicenseUsageHistory.User));
                searchQuery = searchQuery.Sort(s => s.Descending(scoreFieldName).Ascending(userFieldName));
            }
            
            var queryFilterDescriptor = new QueryContainerDescriptor<Entities.LicenseUsageHistory>();
            var queryFilter = new QueryContainer();
            if (!string.IsNullOrEmpty(input.AdvancedFilter))
                queryFilter = queryFilterDescriptor.Raw(input.AdvancedFilter);

            var endTimeFieldName = ElasticSearchUtils.GetCamelCaseField(nameof(Entities.LicenseUsageHistory.EndTime));
            queryFilter &= queryFilterDescriptor.Exists(e => e.Field(endTimeFieldName));
            searchQuery = searchQuery
                .TrackScores()
                .Query(s => queryFilter);

            var esResponse = await _elasticClient.SearchAsync<Entities.LicenseUsageHistory>(searchQuery);
            var items = _mapper.Map<List<LicenseUsageHistoryOutput>>(esResponse.Documents.ToList());
            
            // Get normalized DateTime
            foreach (var item in items)
            {
                var match = esResponse.Documents.FirstOrDefault(l => l.Id == item.Id);
                if (match == null) continue;
                item.AccessDuration = match.AccessDuration(_dateTimeProvider);
                item.AccessDurationInMinutes = match.AccessDurationInMinutes(_dateTimeProvider);
            }
            
            var totalCount = (int) esResponse.Total;
            
            if (totalCount > ElasticSearchConsts.MaxSearchItemsToSearch)
                totalCount = ElasticSearchConsts.MaxSearchItemsToSearch;
            
            return new PagedResultDto<LicenseUsageHistoryOutput>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        private static string GetSortingField(string fieldName)
        {
            var propertyToSort =
                typeof(Entities.LicenseUsageHistory).GetProperty(ElasticSearchUtils.GetPascalCaseField(fieldName));
            if (propertyToSort == null)
                throw new Exception($"Invalid value for: {nameof(PagedAndSortedResultRequestDto.Sorting)} - Field {fieldName} not found");
            var isRawProperty = propertyToSort.PropertyType == typeof(string) || propertyToSort.PropertyType == typeof(Guid);
            var fieldToSort = isRawProperty
                ? ElasticSearchUtils.GetEsFieldRaw(fieldName)
                : ElasticSearchUtils.GetCamelCaseField(fieldName);
            return fieldToSort;
        }
    }
}