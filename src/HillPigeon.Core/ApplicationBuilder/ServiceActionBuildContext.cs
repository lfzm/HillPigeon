using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.ApplicationBuilder
{
    public class ServiceActionBuildContext
    {
        public ServiceActionBuildContext(ActionModel action, ServiceControllerBuildContext controllerBuildContext)
        {
            this.ActionModel = action;
            this.ControllerBuildContext = controllerBuildContext;
        }
        public ActionModel ActionModel { get; }

        public ServiceControllerBuildContext ControllerBuildContext { get; }
    }
}
