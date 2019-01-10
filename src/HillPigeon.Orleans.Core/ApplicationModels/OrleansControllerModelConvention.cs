using HillPigeon.ApplicationModels;
using HillPigeon.Orleans.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.Linq;

namespace HillPigeon.Orleans.ApplicationModels
{
    public class OrleansControllerModelConvention : IControllerModelConvention
    {
        private readonly OrleansRoutingOptions _options;
        public OrleansControllerModelConvention(IOptions<OrleansRoutingOptions> options)
        {
            _options = options.Value;
        }
        public void Apply(ControllerModel controllerModel)
        {
            if (!controllerModel.ControllerType.IsGrain())
            {
                return;
            }

            this.ConversionMacAttribute(controllerModel);
            this.SetControllerName(controllerModel);
            this.SetRoute(controllerModel);
        }

        private void ConversionMacAttribute(ControllerModel controllerModel)
        {
            HillPigeonMvcAttributeConversion.Conversion(controllerModel.Attributes);
        }
        private void SetControllerName(ControllerModel controllerModel)
        {
            if (_options.ControllerNameRule != null)
            {
                controllerModel.ControllerName = _options.ControllerNameRule(controllerModel);
            }
        }
        private void SetRoute(ControllerModel controllerModel)
        {
            if (controllerModel.Attributes.Where(attr => typeof(IRouteTemplateProvider).IsAssignableFrom(attr.GetType())).Count() == 0)
            {
                var routeAttr = new RouteAttribute(controllerModel.ControllerName);
                controllerModel.Attributes.Add(routeAttr);
            }

        }
    }
}
