using System.Reflection.Emit;

namespace HillPigeon.ApplicationBuilder
{
    public interface IServiceActionBuilder
    {
        void Build(TypeBuilder builder, ServiceActionBuildContext context);
    }
}
