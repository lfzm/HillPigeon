using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.Orleans.Core
{
    public class HttpRequestContexBuilder
    {
        public static HttpRequestContext Build(HttpContext context)
        {
            var request = new HttpRequestContext();
            return request;
        }
    }
}
