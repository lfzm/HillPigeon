using HillPigeon.ApplicationBuilder;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace HillPigeon
{
    public class ActionModel
    {
        public ActionModel(MethodInfo methodInfo)
        {
            this.ActionName = methodInfo.Name;
            this.MethodInfo = methodInfo;
            this.ReturnType = methodInfo.ReturnType;
            this.Parameters = new List<ParameterModel>();
            this.Attributes = new List<Attribute>();
        }
        public string ActionName { get; set; }
        public Type ReturnType { get; set; }
        public MethodInfo MethodInfo { get; }
        public IList<ParameterModel> Parameters { get; }
        public IList<Attribute> Attributes { get; }
        public ControllerModel Controller { get; set; }
        public Action<ServiceActionBuildContext, ILGenerator> GeneratActionIL { get; set; }
    }
}
