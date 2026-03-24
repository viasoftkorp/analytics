using Microsoft.AspNetCore.Http;
using Viasoft.Core.AmbientData;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.AspNetCore.AmbientData;

namespace Viasoft.Analytics.UserBehaviour.Host.AmbientData
{
    public class NullEnvironmentAmbientDataCallOptionsResolver: AmbientDataCallOptionsResolver
    {
        public NullEnvironmentAmbientDataCallOptionsResolver(IAmbientData ambientData, IHttpContextAccessor httpContextAccessor): base(ambientData, httpContextAccessor)
        {
        }

        public override AmbientDataCallOptions GetOptions()
        {
            var options = base.GetOptions();
            options.EnvironmentId = null;
            return options;
        }
    }
}