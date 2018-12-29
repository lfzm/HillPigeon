using HillPigeon.ApplicationBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace HillPigeon
{
    public class ActionModel
    {
        public ActionModel( MethodInfo methodInfo)
        {
            this.ActionName = methodInfo.Name;
            this.MethodInfo = methodInfo;
            this.ReturnType = methodInfo.ReturnType;
            this.HttpMethods = new List<AcceptVerbsAttribute>();
            this.Parameters = new List<ParameterModel>();
            this.Routes = new List<RouteAttribute>();
            this.Authorizes = new List<AuthorizeAttribute>();
        }
        public string ActionName { get; set; }
        public Type ReturnType { get; set; }
        public MethodInfo MethodInfo { get; }
        public bool AllowAnonymous { get; set; }
        public IList<ParameterModel> Parameters { get; }
        public IList<RouteAttribute> Routes { get; }
        public IList<AuthorizeAttribute> Authorizes { get; }
        public IList<AcceptVerbsAttribute> HttpMethods { get; }
        public ControllerModel Controller{ get;  set; }
        public Action<ServiceActionBuildContext, ILGenerator> GeneratActionIL { get; set; }
    }
}
