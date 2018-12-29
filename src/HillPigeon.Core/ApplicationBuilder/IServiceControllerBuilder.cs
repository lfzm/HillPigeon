using System.Reflection;
using System.Threading.Tasks;
using System;
namespace HillPigeon.ApplicationBuilder
{
    public interface IServiceControllerBuilder:IDisposable
    {
        Task<TypeInfo> Build(ServiceControllerBuildContext context);
    }
}
