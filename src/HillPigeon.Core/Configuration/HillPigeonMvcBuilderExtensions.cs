using HillPigeon;
using HillPigeon.ApplicationParts;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HillPigeonMvcBuilderExtensions
    {
        public static IMvcBuilder AddHillPigeon(this IMvcBuilder builder, Action<HillPigeonApplicationPartManager> config)
        {
            builder.Services.AddHillPigeonCore(config);
            builder.PartManager.FeatureProviders.Add(new ServiceControllerFeatureProvider(builder.Services.BuildServiceProvider()));
            return builder;
        }
      
    }
}
