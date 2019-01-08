using HillPigeon.ApplicationModels;
using HillPigeon.DependencyInjection;
using HillPigeon.Orleans.Core;
using HillPigeon.Orleans.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HillPigeon
{
    public static class ServiceCollectionExtensions
    {
        public static IHillPigeonBuilder AddOrleansCore(this IHillPigeonBuilder builder, Action<OrleansRouteingOptions> setupAction)
        {
            builder.Services.Configure<OrleansRouteingOptions>(setupAction);
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
