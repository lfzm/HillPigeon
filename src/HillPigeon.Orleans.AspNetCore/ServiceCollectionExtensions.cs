using HillPigeon.DependencyInjection;
using HillPigeon.Orleans.AspNetCore;
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

                    build.PostConfigureHttpGateway();
                    build.AddOrleans();
                });
            });
            builder.AddStartupTask<HttpGatewayStartup>();
            return builder;
        }
    }
}
