using Orleans;
using System;

namespace HillPigeon.Orleans
{
    public class DefaultClusterClientFactory : IClusterClientFactory
    {
        public IClusterClient Create()
        {
            throw new NotImplementedException();
        }
    }
}
