using Orleans;

namespace HillPigeon.Orleans.Core
{
    public interface IClusterClientFactory
    {
        IClusterClient Create();
    }
}
