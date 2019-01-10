using Orleans;

namespace HillPigeon.Orleans
{
    public interface IClusterClientFactory
    {
        IClusterClient Create();
    }
}
