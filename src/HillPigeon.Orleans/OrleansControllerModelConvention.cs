using HillPigeon.ApplicationModels;
using HillPigeon.Orleans.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.Linq;

namespace HillPigeon.Orleans
{
    public class OrleansControllerModelConvention : IControllerModelConvention
    {
        private readonly OrleansRouteingOptions _options;
        public OrleansControllerModelConvention(IOptions<OrleansRouteingOptions> options)
        {
            _options = options.Value;
        }
        public void Apply(ControllerModel controllerModel)
        {
            if (!OrleansGrainHelper.IsGrain(controllerModel.ControllerType))
            {
                return;
            }
            if (_options.ControllerNameRuleFunction != null)
            {
                controllerModel.ControllerName = _options.ControllerNameRuleFunction(controllerModel);
            }
            if (controllerModel.Attributes.Where(attr => typeof(IRouteTemplateProvider).IsAssignableFrom(attr.GetType())).Count() == 0)
            {
                var routeAttr = new RouteAttribute(controllerModel.ControllerName);
                controllerModel.Attributes.Add(routeAttr);
            }
        }
    }
}
