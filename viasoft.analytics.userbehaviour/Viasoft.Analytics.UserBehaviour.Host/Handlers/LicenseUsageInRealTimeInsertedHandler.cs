using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Analytics.UserBehaviour.Domain.Entities;
using Viasoft.Analytics.UserBehaviour.Domain.Enums;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Message;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Handlers
{    
    public class LicenseUsageInRealTimeInsertedHandler : IHandleMessages<LicenseUsageInRealTimeInsertedMessage>, IDisposable
    {
        private readonly Core.DDD.Repositories.IRepository<LicenseUsageHistory> _licenseUsageHistories;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IServiceBus _serviceBus;
        private readonly StringBuilder _stringBuilder;
        private readonly MD5 _md5Hash;
        
        
        public LicenseUsageInRealTimeInsertedHandler(Core.DDD.Repositories.IRepository<LicenseUsageHistory> licenseUsageHistories, IMapper mapper, IUnitOfWork unitOfWork, 
            IDateTimeProvider dateTimeProvider, IServiceBus serviceBus)
        {
            _licenseUsageHistories = licenseUsageHistories;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _serviceBus = serviceBus;
            _md5Hash = MD5.Create();
            _stringBuilder = new StringBuilder();
        }

        public async Task Handle(LicenseUsageInRealTimeInsertedMessage message)
        {
            var now = _dateTimeProvider.UtcNow();
            
            var newLicenseUsages = _mapper.Map<List<LicenseUsageHistory>>(message.InsertedLicensesUsages);

            foreach (var licenseUsageHistory in newLicenseUsages)
            {
                licenseUsageHistory.Hash = CreateHash(licenseUsageHistory);
            }

            var insertedUsageHistories = await InsertNewLicensesUsage(message, newLicenseUsages, now);
            
            var newLicenseUsagesHashes = newLicenseUsages.Select(x => x.Hash).ToList();
            await UpdateUsageTimeForReleasedLicenses(newLicenseUsagesHashes, message.LicensingIdentifier, now);

            await _serviceBus.SendLocal(new LicenseUsageInRealTimeElasticSearchUpdateMessage(insertedUsageHistories, newLicenseUsagesHashes, now, message.LicensingIdentifier));
        }

        private async Task UpdateUsageTimeForReleasedLicenses(List<string> licenseUsagesHashes, Guid licensingIdentifier, DateTime now)
        {
            using (_unitOfWork.Begin(opt => opt.LazyTransactionInitiation = false))
            {
                await _licenseUsageHistories.BatchUpdateAsync(history => new LicenseUsageHistory
                    {
                        EndTime = now
                    },
                    p => !licenseUsagesHashes.Contains(p.Hash) && p.EndTime == null && p.LicensingIdentifier == licensingIdentifier 
                         && p.LicensingMode != LicensingModes.Offline);
                await _unitOfWork.CompleteAsync();
            }
        }

        private async Task<List<LicenseUsageHistory>> InsertNewLicensesUsage(LicenseUsageInRealTimeInsertedMessage message, List<LicenseUsageHistory> licenseUsages, DateTime now)
        {
            // o servidor de licenças envia todas as licenças que estão sendo utilizadas naquele momento, a cada 1 minuto (se houver mudança no uso de licenças)
            // então na hora que essas licenças chegam aqui, precisa descobrir quais licenças que são novas (que começaram a ser usadas agora) e quais que deixaram de ser utilizadas
            // para isso ser possível, precisamos pegar os hashes do banco para saber exatamente o que é novo
            // para não pegar todas as licenças que estão no banco, pegamos somente uma semana para tras. isso porque existem milhares de hashes no banco e o custo de processamento
            // disso é alto. isso só causaria inconsitência de dados caso uma empresa parasse de enviar o uso de licenças por mais de uma seamana
            
            var lastWeek = now.Date.Subtract(TimeSpan.FromDays(7));

            // essa query utiliza um INDEX configurado no DbContext. se alterar a query, precisa rever o índice
            var licenseUsageHistoryHashes = await _licenseUsageHistories.Where(l => l.LicensingIdentifier == message.LicensingIdentifier)
                .Where(history => history.StartTime >= lastWeek)
                .Select(l => l.Hash)
                .AsNoTracking()
                .ToListAsync();

            var hashes = new HashSet<string>(licenseUsageHistoryHashes);
            
            var insertedUsageHistories = new List<LicenseUsageHistory>();
            
            using (_unitOfWork.Begin())
            {
                foreach (var insertedLicenseUsage in licenseUsages)
                {
                    if (!hashes.Contains(insertedLicenseUsage.Hash))
                    {
                        if (message.AccountName != null && message.AccountId != Guid.Empty)
                        {
                            insertedLicenseUsage.AccountName = message.AccountName;
                            insertedLicenseUsage.AccountId = message.AccountId;
                        }

                        insertedUsageHistories.Add(await _licenseUsageHistories.InsertAsync(insertedLicenseUsage));
                    }
                }
                await _unitOfWork.CompleteAsync();
            }

            return insertedUsageHistories;
        }

        private string CreateHash(LicenseUsageHistory input)
        {
            var startTimeConcatenatedWithUser = input.StartTime + input.User + input.AppName + input.LicensingIdentifier;
            
            var data = _md5Hash.ComputeHash(Encoding.UTF8.GetBytes(startTimeConcatenatedWithUser));
            foreach (var t in data)
            {
                _stringBuilder.Append(t.ToString("x2"));
            }

            var hash = _stringBuilder.ToString();
            _stringBuilder.Clear();

            return hash;
        }

        public void Dispose()
        {
            _md5Hash.Dispose();
        }
    }
}