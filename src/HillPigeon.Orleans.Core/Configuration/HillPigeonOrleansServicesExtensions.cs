using HillPigeon.ApplicationModels;
using HillPigeon.Orleans.Core;
using HillPigeon.Orleans.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HillPigeon
{
    public static class HillPigeonOrleansServicesExtensions
    {
        public static IServiceCollection AddOrleansCore(this IServiceCollection services, Action<OrleansRouteingOptions> setupAction)
        {
            OrleansRouteingOptions options = new OrleansRouteingOptions();
            setupAction.Invoke(options);

            services.Configure<OrleansRouteingOptions>(setupAction);
            services.AddHillPigeonCore(options.ApplicationPartManager);

            services.AddSingleton<OrleansActionILGeneratFactory>();
            services.AddSingleton<IActionModelConvention, OrleansActionModelConvention>();
            services.AddSingleton<IControllerModelConvention, OrleansControllerModelConvention>();
            services.AddSingleton<IParameterModelConvention, OrleansParameterModelConvention>();
            services.AddSingleton<IApplicationFeatureProvider, OrleansApplicationFeatureProvider>();
            return services;
        }
    }
}
