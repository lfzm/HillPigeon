using HillPigeon.ApplicationModels;
using HillPigeon.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IHillPigeonBuilderExtensions
    {
        public static IHillPigeonBuilder AddActionModelConvention<TConvention>(this IHillPigeonBuilder builder)
            where TConvention : IActionModelConvention
        {
            builder.Services.AddSingleton(typeof(IActionModelConvention), typeof(TConvention));
            return builder;
        }

        public static IHillPigeonBuilder AddControllerModelConvention<TConvention>(this IHillPigeonBuilder builder)
          where TConvention : IControllerModelConvention
        {
            builder.Services.AddSingleton(typeof(IControllerModelConvention), typeof(TConvention));
            return builder;
        }

        public static IHillPigeonBuilder AddParameterModelConvention<TConvention>(this IHillPigeonBuilder builder)
          where TConvention : IParameterModelConvention
        {
            builder.Services.AddSingleton(typeof(IParameterModelConvention), typeof(TConvention));
            return builder;
        }
        public static IHillPigeonBuilder AddApplicationFeatureProvider<TConvention>(this IHillPigeonBuilder builder)
            where TConvention : IApplicationFeatureProvider
        {
            builder.Services.AddSingleton(typeof(IApplicationFeatureProvider), typeof(TConvention));
            return builder;
        }
    }
}
