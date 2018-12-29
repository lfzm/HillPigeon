using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace HillPigeon.ApplicationBuilder
{
    public class ServiceControllerBuildContext
    {
        public ServiceControllerBuildContext(ControllerModel controller)
        {
            this.Controller = controller;
        }
        public ControllerModel Controller { get; }
        public TypeInfo DynamicTypeInfo { get; internal set; }
        public FieldBuilder ServiceProviderField { get; internal set; }
    }
}
