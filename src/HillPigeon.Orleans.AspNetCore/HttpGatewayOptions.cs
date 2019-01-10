using Microsoft.Extensions.DependencyInjection;
using System;

namespace HillPigeon.Orleans.AspNetCore
{
    public class HttpGatewayOptions
    {
        public Action<IMvcCoreBuilder> MvcBuilderAction { get; set; }

        public string Host { get; set; } = "*";

        public int Port { get; set; } = 8081;
    }
}
