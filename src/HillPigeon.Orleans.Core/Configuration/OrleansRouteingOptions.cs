using HillPigeon.ApplicationParts;
using Microsoft.AspNetCore.Http;
using System;

namespace HillPigeon.Orleans.Core.Configuration
{
    public class OrleansRouteingOptions
    {
        public Func<ControllerModel, string> ControllerNameRule { get; set; }
        public Action<HillPigeonApplicationPartManager> ApplicationPartManager { get; set; }
        public Action<HttpContext, HttpRequestContext> HttpRequestContext { get; set; }
    }
}
