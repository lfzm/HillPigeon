using HillPigeon.DependencyInjection;
using HillPigeon.Orleans.AspNetCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HillPigeonBuilderExtensions
    {
        private static readonly string httpGatewayOptionsKey = "__HttpGatewayOptions";
        public static IHillPigeonBuilder ConfigureHttpGateway(this IHillPigeonBuilder builder, Action<HttpGatewayOptions> startupAction)
        {
            HttpGatewayOptions options = new HttpGatewayOptions();
            startupAction.Invoke(options);
            if (builder.Properties.ContainsKey(httpGatewayOptionsKey))
            {
                builder.Properties[httpGatewayOptionsKey] = options;
            }
            else
            {
                builder.Properties.Add(httpGatewayOptionsKey, options);
            }
            return builder;
        }

        internal static IHillPigeonBuilder PostConfigureHttpGatewayOptions(this IHillPigeonBuilder builder)
        {
            HttpGatewayOptions httpGatewayOptions;
            if (builder.Properties.ContainsKey(httpGatewayOptionsKey))
            {
                httpGatewayOptions = (HttpGatewayOptions)builder.Properties[httpGatewayOptionsKey];
            }
            else
            {
                httpGatewayOptions = new HttpGatewayOptions();
            }
            builder.Services.ConfigureOptions(httpGatewayOptions);
            return builder;
        }
    }
}
