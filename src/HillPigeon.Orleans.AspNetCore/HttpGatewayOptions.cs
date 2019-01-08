using HillPigeon.Orleans.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.Orleans.AspNetCore
{
    public class HttpGatewayOptions : OrleansRouteingOptions
    {
        public Action<IMvcCoreBuilder> MvcBuilderAction { get; set; }

        public string Host { get; set; } = "*";

        public int Port { get; set; } = 8081;
    }
}
