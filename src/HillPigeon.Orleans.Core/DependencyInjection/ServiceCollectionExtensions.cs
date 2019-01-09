using HillPigeon.DependencyInjection;
using HillPigeon.Orleans.Core;
using HillPigeon.Orleans.Core.Configuration;
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
            builder.PostConfigureOrleansRoutingOptions();
            builder.AddOrleansCore();
            return builder;
        }
        public static IHillPigeonBuilder AddOrleansCore(this IHillPigeonBuilder builder)
        {
            builder.Services.AddSingleton<OrleansActionILGeneratFactory>();
            //配置HillPigeon组件
            builder.AddActionModelConvention<OrleansActionModelConvention>();
            builder.AddControllerModelConvention<OrleansControllerModelConvention>();
            builder.AddParameterModelConvention<OrleansParameterModelConvention>();
            builder.AddApplicationFeatureProvider<OrleansApplicationFeatureProvider>();
            return builder;
        }
    }
}
