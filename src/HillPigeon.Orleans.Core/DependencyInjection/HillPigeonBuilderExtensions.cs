using HillPigeon.DependencyInjection;
using HillPigeon.Orleans.Core.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HillPigeonBuilderExtensions
    {
        private static readonly string orleansRouteingOptionsKey = "__OrleansRouteingOptions";
        public static IHillPigeonBuilder ConfigureOrleansRouting(this IHillPigeonBuilder builder, Action<OrleansRoutingOptions> startupAction)
        {
            OrleansRoutingOptions options = new OrleansRoutingOptions();
            startupAction.Invoke(options);
            if (builder.Properties.ContainsKey(orleansRouteingOptionsKey))
            {
                builder.Properties[orleansRouteingOptionsKey] = options;
            }
            else
            {
                builder.Properties.Add(orleansRouteingOptionsKey, options);
            }
            return builder;
        }

        internal static IHillPigeonBuilder PostConfigureOrleansRoutingOptions(this IHillPigeonBuilder builder)
        {
            OrleansRoutingOptions routingOptions;
            if (builder.Properties.ContainsKey(orleansRouteingOptionsKey))
            {
                routingOptions = (OrleansRoutingOptions)builder.Properties[orleansRouteingOptionsKey];
            }
            else
            {
                routingOptions = new OrleansRoutingOptions();
            }
            builder.Services.ConfigureOptions(routingOptions);
            return builder;
        }
    }
}
