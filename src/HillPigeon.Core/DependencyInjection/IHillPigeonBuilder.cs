using HillPigeon.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace HillPigeon.DependencyInjection
{
    public interface IHillPigeonBuilder
    {
        IDictionary<object, object> Properties { get; }
        IApplicationPartManager PartManager { get; }
        IServiceCollection Services { get; }
        void Build();
    }
}
