using Orleans.Runtime;

namespace HillPigeon.Orleans
{
    public class HttpRequestContextAccessor : IHttpRequestContextAccessor
    {
        public HttpRequestContext HttpRequestContext => (HttpRequestContext)RequestContext.Get("HttpRequest");
    }
}
