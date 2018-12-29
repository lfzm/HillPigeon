using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace HillPigeon
{
    public class ControllerModel
    {
        public ControllerModel(string moduleName,Type controllerType )
        {
            this.ControllerName = controllerType.Name;
            this.ModuleName = moduleName;
            this.ControllerType = controllerType;
            this.Actions = new List<ActionModel>();
            this.Routes = new List<RouteAttribute>();
            this.Authorizes = new List<AuthorizeAttribute>();
        }

        public string ControllerName { get; set; }
        public string ModuleName { get; set; }
        public Type ControllerType { get; }
        public IList<ActionModel> Actions { get; }
        public IList<RouteAttribute> Routes { get; }
        public IList<AuthorizeAttribute> Authorizes { get; }

        public ApplicationModel Application { get; set; }


    }
}
