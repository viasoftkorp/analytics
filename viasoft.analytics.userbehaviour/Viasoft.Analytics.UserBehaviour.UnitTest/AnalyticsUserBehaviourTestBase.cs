using System;
using Viasoft.Analytics.UserBehaviour.Host.Mappers;
using Viasoft.Analytics.UserBehaviour.Infrastructure.PostgreSql.EntityFrameworkCore;
using Viasoft.Core.Mapper.Extensions;
using Viasoft.Core.Testing;

namespace Viasoft.Analytics.UserBehaviour.Testing;

public class AnalyticsUserBehaviourTestBase : UnitTestBase
{
    protected override Type GetDbContextType()
    {
        return typeof(ViasoftAnalyticsUserBehaviourDbContext);
    }
    
    protected override void AddServices()
    {
        ServiceCollection.AddAutoMapper(new []{typeof(UsageHistoryIndexProfile).Assembly});
        base.AddServices();
    }
}