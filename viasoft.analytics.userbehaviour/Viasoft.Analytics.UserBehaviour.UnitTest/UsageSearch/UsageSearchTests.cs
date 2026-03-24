using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSubstitute;
using Viasoft.Analytics.UserBehaviour.Domain.Enums;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.DTO;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Enum;
using Viasoft.Analytics.UserBehaviour.Domain.LicenseUsageHistory.Service;
using Viasoft.Analytics.UserBehaviour.Domain.Services.DateIntervalProviders;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.Testing;
using Xunit;

namespace Viasoft.Analytics.UserBehaviour.Testing.UsageSearch
{
    public class UsageSearchTests: AnalyticsUserBehaviourTestBase
    {
        [Fact]
        public async Task MustReturnDictionaryWithNoTenantFilterAndTenantGrouping()
        {
            //arrange
            var usageHistories = new List<Domain.Entities.LicenseUsageHistory>
            {
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "60768737000180",
                    Domain = Domains.Accounting,
                    Language = "pt-br",
                    User = "User",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste",
                    AdditionalLicense = false,
                    AdditionalLicenses = 0,
                    AppIdentifier = "LS03",
                    AppLicenses = 10,
                    AppName = "Monitor de licenças",
                    AppStatus = LicensedAppStatus.AppActive,
                    LicensingIdentifier = Guid.NewGuid(),
                    BrowserInfo = "Chrome",
                    BundleIdentifier = null,
                    BundleName = null,
                    LicensingStatus = LicensingStatus.Active
                },
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "77847508000154",
                    Domain = Domains.Development,
                    Language = "pt-br",
                    User = "Not a user",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste2",
                    AdditionalLicense = true,
                    AdditionalLicenses = 100,
                    AppIdentifier = "LS01",
                    AppLicenses = 10,
                    AppName = "Licenciamento",
                    AppStatus = LicensedAppStatus.AppActive,
                    BrowserInfo = "Edge",
                    BundleIdentifier = "BUNDLE",
                    BundleName = "bundle",
                    LicensingIdentifier = Guid.NewGuid(),
                    LicensingStatus = LicensingStatus.Active
                }
            };
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            foreach (var usageHistory in usageHistories)
            {
                await repo.InsertAsync(usageHistory, true);
            }
            var esMock = Substitute.For<IElasticClient>();
            var tenantMock = Substitute.For<ICurrentTenant>();
            var dateIntervalProvider = Substitute.For<IDateIntervalProvider>();
            tenantMock.Id.Returns(Guid.Parse("8814BD7A-D9FA-4529-95EC-BADE10C4C3C2"));
            var esServiceMock = new Mock<IFilterByGroupingEsService>();
            var expectedResult = new List<FilterByGroupingKey>
            {
                new FilterByGroupingKey
                {
                    AccountId = usageHistories[0].AccountId,
                    AccountName = usageHistories[0].AccountName,
                    Value = 1,
                    LicensingIdentifier = usageHistories[0].LicensingIdentifier
                },
                new FilterByGroupingKey
                {
                    AccountId = usageHistories[1].AccountId,
                    AccountName = usageHistories[1].AccountName,
                    Value = 1,
                    LicensingIdentifier = usageHistories[1].LicensingIdentifier
                }
            };
            esServiceMock.Setup(s => s.NormalizeEsResult(
                It.IsAny<string>(),
                It.IsAny<Groupings>(),
                It.IsAny<ISearchResponse<Domain.Entities.LicenseUsageHistory>>()
            )).Returns(expectedResult);
            var service = new FilterByGroupingService(esMock, tenantMock, esServiceMock.Object, dateIntervalProvider);

            //act
            var testTenant = await service.FilterByGrouping(new FilterByGroupingInput
            {
                Groupings = Groupings.Tenant
            });

            //assert
            testTenant.Values.Should().BeEquivalentTo(expectedResult);
            Assert.Single(esServiceMock.Invocations
                .Where(w => w.Method.Name == nameof(IFilterByGroupingEsService.NormalizeEsResult)));
        }

        [Fact]
        public async Task MustReturnDictionaryWithNoTenantFilterAndDomainGrouping()
        {
            //arrange
            var usageHistories = new List<Domain.Entities.LicenseUsageHistory>
            {
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "60768737000180",
                    Domain = Domains.Accounting,
                    Language = "pt-br",
                    User = "User",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste",
                    AdditionalLicense = false,
                    AdditionalLicenses = 0,
                    AppIdentifier = "LS03",
                    AppLicenses = 10,
                    AppName = "Monitor de licenças",
                    AppStatus = LicensedAppStatus.AppActive,
                    LicensingIdentifier = Guid.NewGuid(),
                    BrowserInfo = "Chrome",
                    BundleIdentifier = null,
                    BundleName = null,
                    LicensingStatus = LicensingStatus.Active
                },
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "77847508000154",
                    Domain = Domains.Development,
                    Language = "pt-br",
                    User = "Not a user",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste2",
                    AdditionalLicense = true,
                    AdditionalLicenses = 100,
                    AppIdentifier = "LS01",
                    AppLicenses = 10,
                    AppName = "Licenciamento",
                    AppStatus = LicensedAppStatus.AppActive,
                    BrowserInfo = "Edge",
                    BundleIdentifier = "BUNDLE",
                    BundleName = "bundle",
                    LicensingIdentifier = Guid.NewGuid(),
                    LicensingStatus = LicensingStatus.Active
                }
            };
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            foreach (var usageHistory in usageHistories)
            {
                await repo.InsertAsync(usageHistory, true);
            }
            var esMock = Substitute.For<IElasticClient>();
            var tenantMock = Substitute.For<ICurrentTenant>();
            var dateIntervalProvider = Substitute.For<IDateIntervalProvider>();
            tenantMock.Id.Returns(Guid.Parse("8814BD7A-D9FA-4529-95EC-BADE10C4C3C2"));
            var expectedResult = new List<FilterByGroupingKey>
            {
                new FilterByGroupingKey
                {
                    Domain = usageHistories[0].Domain,
                    Value = 1
                },
                new FilterByGroupingKey
                {
                    Domain = usageHistories[1].Domain,
                    Value = 1
                }
            };
            var esServiceMock = new Mock<IFilterByGroupingEsService>();
            esServiceMock.Setup(s => s.NormalizeEsResult(
                It.IsAny<string>(),
                It.IsAny<Groupings>(),
                It.IsAny<ISearchResponse<Domain.Entities.LicenseUsageHistory>>()
            )).Returns(expectedResult);
            var service = new FilterByGroupingService(esMock, tenantMock, esServiceMock.Object, dateIntervalProvider);
            
            //act
            var testDomain = await service.FilterByGrouping(new FilterByGroupingInput
            {
                Groupings = Groupings.Domain
            });
            
            //assert
            testDomain.Values.Should().BeEquivalentTo(expectedResult);
            Assert.Single(esServiceMock.Invocations
                .Where(w => w.Method.Name == nameof(IFilterByGroupingEsService.NormalizeEsResult)));
        }

        [Fact]
        public async Task MustReturnDictionaryWithNoTenantFilterAndAppGrouping()
        {
            //arrange
            var usageHistories = new List<Domain.Entities.LicenseUsageHistory>
            {
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "60768737000180",
                    Domain = Domains.Accounting,
                    Language = "pt-br",
                    User = "User",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste",
                    AdditionalLicense = false,
                    AdditionalLicenses = 0,
                    AppIdentifier = "LS03",
                    AppLicenses = 10,
                    AppName = "Monitor de licenças",
                    AppStatus = LicensedAppStatus.AppActive,
                    LicensingIdentifier = Guid.NewGuid(),
                    BrowserInfo = "Chrome",
                    BundleIdentifier = null,
                    BundleName = null,
                    LicensingStatus = LicensingStatus.Active
                },
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "77847508000154",
                    Domain = Domains.Development,
                    Language = "pt-br",
                    User = "Not a user",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste2",
                    AdditionalLicense = true,
                    AdditionalLicenses = 100,
                    AppIdentifier = "LS01",
                    AppLicenses = 10,
                    AppName = "Licenciamento",
                    AppStatus = LicensedAppStatus.AppActive,
                    BrowserInfo = "Edge",
                    BundleIdentifier = "BUNDLE",
                    BundleName = "bundle",
                    LicensingIdentifier = Guid.NewGuid(),
                    LicensingStatus = LicensingStatus.Active
                }
            };
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            foreach (var usageHistory in usageHistories)
            {
                await repo.InsertAsync(usageHistory, true);
            }
            var esMock = Substitute.For<IElasticClient>();
            var tenantMock = Substitute.For<ICurrentTenant>();
            var dateIntervalProvider = Substitute.For<IDateIntervalProvider>();
            tenantMock.Id.Returns(Guid.Parse("8814BD7A-D9FA-4529-95EC-BADE10C4C3C2"));
            var esServiceMock = new Mock<IFilterByGroupingEsService>();
            var expectedResult = new List<FilterByGroupingKey>
            {
                new FilterByGroupingKey
                {
                    AppIdentifier = usageHistories[0].AppIdentifier,
                    AppName = usageHistories[0].AppName,
                    Value = 1
                },
                new FilterByGroupingKey
                {
                    AppIdentifier = usageHistories[1].AppIdentifier,
                    AppName = usageHistories[1].AppName,
                    Value = 1
                }
            };
            esServiceMock.Setup(s => s.NormalizeEsResult(
                It.IsAny<string>(),
                It.IsAny<Groupings>(),
                It.IsAny<ISearchResponse<Domain.Entities.LicenseUsageHistory>>()
            )).Returns(expectedResult);
            var service = new FilterByGroupingService(esMock, tenantMock, esServiceMock.Object, dateIntervalProvider);
            
            //act
            var testAppGroup = await service.FilterByGrouping(new FilterByGroupingInput
            {
                Groupings = Groupings.App
            });

            //assert
            testAppGroup.Values.Should().BeEquivalentTo(expectedResult);
            Assert.Single(esServiceMock.Invocations
                    .Where(w => w.Method.Name == nameof(IFilterByGroupingEsService.NormalizeEsResult)));
        }

        [Fact]
        public async Task MustReturnDictionaryWithTenantFilterAndTenantGrouping()
        { 
            //arrange
            var usageHistories = new List<Domain.Entities.LicenseUsageHistory>
            {
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "60768737000180",
                    Domain = Domains.Accounting,
                    Language = "pt-br",
                    User = "User",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste",
                    AdditionalLicense = false,
                    AdditionalLicenses = 0,
                    AppIdentifier = "LS03",
                    AppLicenses = 10,
                    AppName = "Monitor de licenças",
                    AppStatus = LicensedAppStatus.AppActive,
                    LicensingIdentifier = Guid.NewGuid(),
                    BrowserInfo = "Chrome",
                    BundleIdentifier = null,
                    BundleName = null,
                    LicensingStatus = LicensingStatus.Active
                },
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "77847508000154",
                    Domain = Domains.Development,
                    Language = "pt-br",
                    User = "Not a user",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste2",
                    AdditionalLicense = true,
                    AdditionalLicenses = 100,
                    AppIdentifier = "LS01",
                    AppLicenses = 10,
                    AppName = "Licenciamento",
                    AppStatus = LicensedAppStatus.AppActive,
                    BrowserInfo = "Edge",
                    BundleIdentifier = "BUNDLE",
                    BundleName = "bundle",
                    LicensingIdentifier = Guid.NewGuid(),
                    LicensingStatus = LicensingStatus.Active
                }
            };
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            foreach (var usageHistory in usageHistories)
            {
                await repo.InsertAsync(usageHistory, true);
            }
            var esMock = Substitute.For<IElasticClient>();
            var tenantMock = Substitute.For<ICurrentTenant>();
            var dateIntervalProvider = Substitute.For<IDateIntervalProvider>();
            tenantMock.Id.Returns(Guid.Parse("8814BD7A-D9FA-4529-95EC-BADE10C4C3C2"));
            var esServiceMock = new Mock<IFilterByGroupingEsService>();
            var expectedResult = new List<FilterByGroupingKey>
            {
                new FilterByGroupingKey
                {
                    AppIdentifier = usageHistories[0].AppIdentifier,
                    AppName = usageHistories[0].AppName,
                    Value = 1
                },
                new FilterByGroupingKey
                {
                    AppIdentifier = usageHistories[1].AppIdentifier,
                    AppName = usageHistories[1].AppName,
                    Value = 1
                }
            };
            esServiceMock.Setup(s => s.NormalizeEsResult(
                It.IsAny<string>(),
                It.IsAny<Groupings>(),
                It.IsAny<ISearchResponse<Domain.Entities.LicenseUsageHistory>>()
            )).Returns(expectedResult);
            var service = new FilterByGroupingService(esMock, tenantMock, esServiceMock.Object, dateIntervalProvider);
            var advancedFilter = new JsonNetFilterRule
            {
                Condition = "and",
                Rules = new List<JsonNetFilterRule>
                {
                    new JsonNetFilterRule
                    {
                        Operator = "equal",
                        Field = "AccountName",
                        Type = "string",
                        Value = usageHistories[0].AccountName
                    }
                }
            };
            var serializedAdvancedFilter = JsonConvert.SerializeObject(advancedFilter, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            
            //act
            var testTenantAndTenantFilter = await service.FilterByGrouping(new FilterByGroupingInput
            {
                Groupings = Groupings.Tenant,
                AdvancedFilter = serializedAdvancedFilter
            });
            
            //assert
            Assert.Single(esServiceMock.Invocations);
        }
        
        [Fact]
        public async Task MustReturnDictionaryWithTenantFilterAndDomainGrouping()
        {
            //arrange
            var usageHistories = new List<Domain.Entities.LicenseUsageHistory>
            {
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "60768737000180",
                    Domain = Domains.Accounting,
                    Language = "pt-br",
                    User = "User",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste",
                    AdditionalLicense = false,
                    AdditionalLicenses = 0,
                    AppIdentifier = "LS03",
                    AppLicenses = 10,
                    AppName = "Monitor de licenças",
                    AppStatus = LicensedAppStatus.AppActive,
                    LicensingIdentifier = Guid.NewGuid(),
                    BrowserInfo = "Chrome",
                    BundleIdentifier = null,
                    BundleName = null,
                    LicensingStatus = LicensingStatus.Active
                },
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "77847508000154",
                    Domain = Domains.Development,
                    Language = "pt-br",
                    User = "Not a user",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste2",
                    AdditionalLicense = true,
                    AdditionalLicenses = 100,
                    AppIdentifier = "LS01",
                    AppLicenses = 10,
                    AppName = "Licenciamento",
                    AppStatus = LicensedAppStatus.AppActive,
                    BrowserInfo = "Edge",
                    BundleIdentifier = "BUNDLE",
                    BundleName = "bundle",
                    LicensingIdentifier = Guid.NewGuid(),
                    LicensingStatus = LicensingStatus.Active
                }
            };
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            foreach (var usageHistory in usageHistories)
            {
                await repo.InsertAsync(usageHistory, true);
            }
            var esMock = Substitute.For<IElasticClient>();
            var tenantMock = Substitute.For<ICurrentTenant>();
            var dateIntervalProvider = Substitute.For<IDateIntervalProvider>();
            tenantMock.Id.Returns(Guid.Parse("8814BD7A-D9FA-4529-95EC-BADE10C4C3C2"));
            var esServiceMock = new Mock<IFilterByGroupingEsService>();
            var expectedResult = new List<FilterByGroupingKey>
            {
                new FilterByGroupingKey
                {
                    Domain = usageHistories[0].Domain,
                    Value = 1
                }
            };
            esServiceMock.Setup(s => s.NormalizeEsResult(
                It.IsAny<string>(),
                It.IsAny<Groupings>(),
                It.IsAny<ISearchResponse<Domain.Entities.LicenseUsageHistory>>()
            )).Returns(expectedResult);
            var service = new FilterByGroupingService(esMock, tenantMock, esServiceMock.Object, dateIntervalProvider);
            var advancedFilter = new JsonNetFilterRule
            {
                Condition = "and",
                Rules = new List<JsonNetFilterRule>
                {
                    new JsonNetFilterRule
                    {
                        Operator = "equal",
                        Field = "AccountName",
                        Type = "string",
                        Value = usageHistories[0].AccountName
                    }
                }
            };
            var serializedAdvancedFilter = JsonConvert.SerializeObject(advancedFilter, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            
            //act
            var testDomainAndTenantFilter = await service.FilterByGrouping(new FilterByGroupingInput
            {
                Groupings = Groupings.Domain,
                AdvancedFilter = serializedAdvancedFilter
            });
            
            //assert
            testDomainAndTenantFilter.Values.Should().BeEquivalentTo(expectedResult);
            Assert.Single(esServiceMock.Invocations
                .Where(w => w.Method.Name == nameof(IFilterByGroupingEsService.NormalizeEsResult)));
        }
        
        [Fact]
        public async Task MustReturnDictionaryWithTenantFilterAndAppGrouping()
        {
            //arrange
            var usageHistories = new List<Domain.Entities.LicenseUsageHistory>
            {
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "60768737000180",
                    Domain = Domains.Accounting,
                    Language = "pt-br",
                    User = "User",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste",
                    AdditionalLicense = false,
                    AdditionalLicenses = 0,
                    AppIdentifier = "LS03",
                    AppLicenses = 10,
                    AppName = "Monitor de licenças",
                    AppStatus = LicensedAppStatus.AppActive,
                    LicensingIdentifier = Guid.NewGuid(),
                    BrowserInfo = "Chrome",
                    BundleIdentifier = null,
                    BundleName = null,
                    LicensingStatus = LicensingStatus.Active
                },
                new Domain.Entities.LicenseUsageHistory
                {
                    Cnpj = "77847508000154",
                    Domain = Domains.Development,
                    Language = "pt-br",
                    User = "Not a user",
                    AccountId = Guid.NewGuid(),
                    AccountName = "AccountTeste2",
                    AdditionalLicense = true,
                    AdditionalLicenses = 100,
                    AppIdentifier = "LS01",
                    AppLicenses = 10,
                    AppName = "Licenciamento",
                    AppStatus = LicensedAppStatus.AppActive,
                    BrowserInfo = "Edge",
                    BundleIdentifier = "BUNDLE",
                    BundleName = "bundle",
                    LicensingIdentifier = Guid.NewGuid(),
                    LicensingStatus = LicensingStatus.Active
                }
            };
            var repo = ServiceProvider.GetService<Core.DDD.Repositories.IRepository<Domain.Entities.LicenseUsageHistory>>();
            foreach (var usageHistory in usageHistories)
            {
                await repo.InsertAsync(usageHistory, true);
            }
            var esMock = Substitute.For<IElasticClient>();
            var tenantMock = Substitute.For<ICurrentTenant>();
            var dateIntervalProvider = Substitute.For<IDateIntervalProvider>();
            tenantMock.Id.Returns(Guid.Parse("8814BD7A-D9FA-4529-95EC-BADE10C4C3C2"));
            var esServiceMock = new Mock<IFilterByGroupingEsService>();
            var expectedResult = new List<FilterByGroupingKey>
            {
                new FilterByGroupingKey
                {
                    AppIdentifier = usageHistories[0].AppIdentifier,
                    AppName = usageHistories[0].AppName,
                    Value = 1
                }
            };
            esServiceMock.Setup(s => s.NormalizeEsResult(
                It.IsAny<string>(),
                It.IsAny<Groupings>(),
                It.IsAny<ISearchResponse<Domain.Entities.LicenseUsageHistory>>()
            )).Returns(expectedResult);
            var service = new FilterByGroupingService(esMock, tenantMock, esServiceMock.Object, dateIntervalProvider);
            var advancedFilter = new JsonNetFilterRule
            {
                Condition = "and",
                Rules = new List<JsonNetFilterRule>
                {
                    new JsonNetFilterRule
                    {
                        Operator = "equal",
                        Field = "AccountName",
                        Type = "string",
                        Value = usageHistories[0].AccountName
                    }
                }
            };
            var serializedAdvancedFilter = JsonConvert.SerializeObject(advancedFilter, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            
            //act
            var testAppGroupAndTenantFilter = await service.FilterByGrouping(new FilterByGroupingInput
            {
                Groupings = Groupings.App,
                AdvancedFilter = serializedAdvancedFilter
            });

            //assert
            testAppGroupAndTenantFilter.Values.Should().BeEquivalentTo(expectedResult);
            Assert.Single(esServiceMock.Invocations
                .Where(w => w.Method.Name == nameof(IFilterByGroupingEsService.NormalizeEsResult)));
        }
    }
}