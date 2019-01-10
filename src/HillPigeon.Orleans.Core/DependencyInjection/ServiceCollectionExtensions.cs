using HillPigeon.DependencyInjection;
using HillPigeon.Orleans;
using HillPigeon.Orleans.ApplicationModels;
using HillPigeon.Orleans.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IHillPigeonBuilder AddOrleans(this IHillPigeonBuilder builder, Action<OrleansRoutingOptions> setupAction)
        {
            builder.Services.Configure<OrleansRoutingOptions>(setupAction);
            builder.AddOrleansCore();
            return builder;
        }

        public static IHillPigeonBuilder AddOrleans(this IHillPigeonBuilder builder, OrleansRoutingOptions options)
        {
            builder.Services.ConfigureOptions(options);
            builder.AddOrleansCore();
            return builder;
        }
        public static IHillPigeonBuilder AddOrleans(this IHillPigeonBuilder builder)
        {
            builder.PostConfigureOrleansRouting();
            builder.AddOrleansCore();
            return builder;
        }
        public static IHillPigeonBuilder AddOrleansCore(this IHillPigeonBuilder builder)
        {
            //配置HillPigeon ApplicationModels组件
            builder.Services.AddSingleton<OrleansActionILGeneratFactory>();
            builder.AddActionModelConvention<OrleansActionModelConvention>();
            builder.AddControllerModelConvention<OrleansControllerModelConvention>();
            builder.AddParameterModelConvention<OrleansParameterModelConvention>();
            builder.AddApplicationFeatureProvider<OrleansApplicationFeatureProvider>();
            //配置Orleans Client
            builder.Services.AddSingleton<IClusterClientFactory, DefaultClusterClientFactory>();
            builder.Services.AddSingleton<IOrleansClient, DefaultOrleansClient>();
            return builder;
        }
    }
}
