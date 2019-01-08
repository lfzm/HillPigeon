using HillPigeon.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace HillPigeon.DependencyInjection
{
    public interface IHillPigeonBuilder
    {
        IApplicationPartManager PartManager { get; }
        IServiceCollection Services { get; }
        void Build();
    }
}
