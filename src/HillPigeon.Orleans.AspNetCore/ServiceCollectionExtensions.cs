using HillPigeon.DependencyInjection;
using Orleans.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ISiloHostBuilder AddHillPigeon(this ISiloHostBuilder builder, Action<IHillPigeonBuilder> startupAction)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHillPigeonCore(build =>
                {
                    startupAction?.Invoke(build);

                    build.PostConfigureHttpGatewayOptions();
                    build.AddOrleans();
                });
            });
            return builder;
        }
    }
}
