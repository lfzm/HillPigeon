using System.Collections.Generic;
using System.Reflection;

namespace HillPigeon
{
    public interface IServiceControllerFactory
    {
        IEnumerable<TypeInfo> Create();
    }
}
