using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Viasoft.Analytics.UserBehaviour.Domain.Enums;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Message;
using Viasoft.Analytics.UserBehaviour.Host.Handlers;
using Viasoft.Analytics.UserBehaviour.Host.Mappers;
using Viasoft.Core.Mapper.Extensions;
using Viasoft.Core.MultiTenancy.Abstractions.Store;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant.Store;
using Viasoft.Core.Testing.DateTimeProvider;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.Handlers
{
    public class LicenseUsageHistoryTests: AnalyticsUserBehaviourTestBase
    {
        [Fact]
        public async Task MustInsertInLicenseUsageHistory()
        {
            //arrange
            var mapper = ServiceProvider.GetService<IMapper>();
            var mockDateTimeA = new FakeDateTimeProvider(DateTime.Parse("2020-05-28"));
            var mockDateTimeB = new FakeDateTimeProvider(DateTime.Parse("2020-05-29"));
            var mockDateA = mockDateTimeA.UtcNow();
            const string accountName = "Account name teste";
            var accountId = Guid.NewGuid();
            var data = new LicenseUsageInRealTimeInsertedMessage
            {
                LicensingIdentifier = Guid.Parse("AC3460F0-C4E4-4363-8F4E-4D63DC596800"),
                AccountId = accountId,
                AccountName = accountName,
                InsertedLicensesUsages = new List<InsertedLicenseUsage>
                {
                    new InsertedLicenseUsage
                    {
                        Cnpj = "83909228000126",
                        Language = null,
                        User = "TestUser@test.com.br",
                        AdditionalLicense = false,
                        AdditionalLicenses = 1,
                        AppIdentifier = "TST",
                        AppLicenses = 10,
                        AppName = "App de testes",
                        AppStatus = LicensedAppStatus.AppActive,
                        BrowserInfo = null,
                        BundleIdentifier = null,
                        BundleName = null,
                        DatabaseName = null,
                        HostName = null,
                        HostUser = null,
                        LastUpdate = mockDateA,
                        LicensingIdentifier = Guid.Parse("AC3460F0-C4E4-4363-8F4E-4D63DC596800"),
                        LicensingStatus = LicensingStatus.Active,
                        OsInfo = null,
                        SoftwareIdentifier = "WEB",
                        SoftwareName = "Sistema web",
                        SoftwareVersion = null,
                        StartTime = mockDateA,
                        AdditionalLicensesConsumed = 0,
                        AppLicensesConsumed = 1,
                        LocalIpAddress = null,
                        Domain = Domains.Financial
                    }
                }
            };
            var entity = mapper.Map<Domain.Entities.LicenseUsageHistory>(data.InsertedLicensesUsages[0]);
            data.InsertedLicensesUsages[0].AccessDuration = entity.AccessDuration(mockDateTimeB);
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            
            var tenancyStoreMock = new Mock<ITenancyStore>();
            tenancyStoreMock
                .Setup(t => t.GetHostTenantAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new TenancyStoreGetHostTenant
                {
                    HostTenantId = data.LicensingIdentifier
                }));


            var handler = new LicenseUsageInRealTimeInsertedHandler(repo, mapper, UnitOfWork, mockDateTimeA, ServiceBus);
            
            //act
            await handler.Handle(data);
            
            //assert
            var insertedData = repo.Single();
            Assert.Single(repo);
            insertedData.Should().BeEquivalentTo(data.InsertedLicensesUsages.First(), options => options.Excluding(su => su.AccessDuration));
            Assert.Equal(data.InsertedLicensesUsages.First().AccessDuration, insertedData.AccessDuration(mockDateTimeB));
            Assert.Equal(accountName, insertedData.AccountName);
            Assert.Equal(insertedData.AccountId, accountId);
        }

        [Fact(Skip = "BatchUpdate")]
        public async Task MustUpdateEndTime()
        {
            //arrange
            var mapper = ServiceProvider.GetService<IMapper>();
            var mockDateTime = new FakeDateTimeProvider(DateTime.Parse("2020-05-28")).UtcNow();
            var accountName = "account name teste";
            var accountId = Guid.NewGuid();
            var data = new LicenseUsageInRealTimeInsertedMessage
            {
                LicensingIdentifier = Guid.Parse("AC3460F0-C4E4-4363-8F4E-4D63DC596800"),
                AccountName = accountName,
                AccountId = accountId,
                InsertedLicensesUsages = new List<InsertedLicenseUsage>
                {
                    new InsertedLicenseUsage
                    {
                        Cnpj = "83909228000126",
                        Language = null,
                        User = "TestUser@test.com.br",
                        AdditionalLicense = false,
                        AdditionalLicenses = 1,
                        AppIdentifier = "TST",
                        AppLicenses = 10,
                        AppName = "App de testes",
                        AppStatus = LicensedAppStatus.AppActive,
                        BrowserInfo = null,
                        BundleIdentifier = null,
                        BundleName = null,
                        DatabaseName = null,
                        HostName = null,
                        HostUser = null,
                        LastUpdate = mockDateTime,
                        LicensingIdentifier = Guid.Parse("AC3460F0-C4E4-4363-8F4E-4D63DC596800"),
                        LicensingStatus = LicensingStatus.Active,
                        OsInfo = null,
                        SoftwareIdentifier = "WEB",
                        SoftwareName = "Sistema web",
                        SoftwareVersion = null,
                        StartTime = mockDateTime,
                        AdditionalLicensesConsumed = 0,
                        AppLicensesConsumed = 1,
                        LocalIpAddress = null
                    }
                }
            };
            var secondaryData = new LicenseUsageInRealTimeInsertedMessage
            {
                LicensingIdentifier = Guid.Parse("AC3460F0-C4E4-4363-8F4E-4D63DC596800"),
                AccountId = accountId,
                AccountName = accountName,
                InsertedLicensesUsages = new List<InsertedLicenseUsage>()
            };
            var entity = mapper.Map<Domain.Entities.LicenseUsageHistory>(data.InsertedLicensesUsages[0]);
            entity.AccountName = accountName;
            entity.AccountId = accountId;
            entity.Hash = "b2475bc425fd920e557a636fc2e89820";
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            var handler = ActivatorUtilities.CreateInstance<LicenseUsageInRealTimeInsertedHandler>(ServiceProvider);
            await repo.InsertAsync(entity, true);
            
            //act
            await handler.Handle(secondaryData);
            
            //assert
            var insertedData = repo.Single();
            Assert.Single(repo);
            insertedData.EndTime.Should().NotBeNull();
            Assert.Equal(accountName, insertedData.AccountName);
            Assert.Equal(accountId, insertedData.AccountId);
        }
    }
}