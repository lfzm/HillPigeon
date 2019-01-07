namespace HillPigeon.Orleans
{
    public interface IHttpRequestContextAccessor
    {
        HttpRequestContext HttpRequestContext { get; }
    }
}
