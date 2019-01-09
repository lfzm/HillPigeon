using HillPigeon.ApplicationParts;
using Microsoft.AspNetCore.Http;
using System;

namespace HillPigeon.Orleans.Core.Configuration
{
    public class OrleansRoutingOptions
    {
        public Func<ControllerModel, string> ControllerNameRule { get; set; }
        public Action<HttpContext, HttpRequestContext> HttpRequestContext { get; set; }
    }
}
