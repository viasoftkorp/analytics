using Microsoft.Extensions.DependencyInjection;
using Viasoft.Analytics.UserBehaviour.Host.AmbientData;
using Viasoft.Core.AmbientData.Abstractions;

namespace Viasoft.Analytics.UserBehaviour.Host.Extensions
{
    public static class AmbientDataExtensions
    {
        public static IServiceCollection AddNullEnvironmentAmbientDataCallOptionsResolver(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAmbientDataCallOptionsResolver, NullEnvironmentAmbientDataCallOptionsResolver>();
            return serviceCollection;
        }
    }
}