using HillPigeon.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IMvcCoreBuilder AddHillPigeon(this IMvcCoreBuilder builder, Action<IHillPigeonBuilder> startupAction)
        {
            builder.Services.AddHillPigeonCore(startupAction);
            return builder;
        }
        public static IMvcBuilder AddHillPigeon(this IMvcBuilder builder, Action<IHillPigeonBuilder> startupAction)
        {
            builder.Services.AddHillPigeonCore(startupAction);
            return builder;
        }
        public static IServiceCollection AddHillPigeonCore(this IServiceCollection services, Action<IHillPigeonBuilder> startupAction)
        {
            IHillPigeonBuilder builder = new HillPigeonBuilder(services);
            startupAction?.Invoke(builder);
            builder.Build();
            return services;
        }
    }
}
