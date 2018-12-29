using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection.Emit;

namespace HillPigeon.ApplicationBuilder
{
    internal class ServiceActionBuilder : IServiceActionBuilder
    {
        public void Build(TypeBuilder builder, ServiceActionBuildContext context)
        {
            builder.BuildMethod(context, this.BuildAction);
        }

        public void BuildAction(MethodBuilder builder, ServiceActionBuildContext  context)
        {
            var actionModel = context.Action;
            builder
                .BuildParameter(actionModel)
                .BuildRouteAttributes(actionModel.Routes)
                .BuildAuthorizeAttributes(actionModel.Authorizes, actionModel.AllowAnonymous)
                .BuildHttpMethodAttributes(actionModel.HttpMethods)
                .BuildMethodContext(context, actionModel.GeneratActionIL);
        }

     
    }
}
