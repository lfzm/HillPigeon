using HillPigeon.ApplicationBuilder;
using HillPigeon.ApplicationModels;
using HillPigeon.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HillPigeon
{
    public static class HillPigeonServicesExtensions
    {
        public static IServiceCollection AddHillPigeonCore(this IServiceCollection services, Action<HillPigeonApplicationPartManager> config)
        {
            services.Configure<HillPigeonApplicationPartManager>(config);
            services.AddSingleton<IServiceControllerFactory, ServiceControllerFactory>();

            //ApplicationBuilder
            services.AddSingleton<IServiceControllerBuilder, ServiceControllerBuilder>();
            services.AddSingleton<IServiceActionBuilder, ServiceActionBuilder>();

            //ApplicationModels
            services.AddSingleton<ApplicationModelFactory>();
            services.AddSingleton<ControllerModelBuilder>();
            services.AddSingleton<ActionModelBuilder>();
            services.AddSingleton<ParameterModelBuilder>();
            services.AddSingleton<IApplicationModelProvider, DefaultApplicationModelProvider>();
            return services;
        }
    }
}
