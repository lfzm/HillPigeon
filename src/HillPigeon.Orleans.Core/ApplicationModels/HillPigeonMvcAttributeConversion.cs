using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HillPigeon.Orleans.ApplicationModels
{
    public class HillPigeonMvcAttributeConversion
    {
        public static void Conversion(IList<Attribute> attributes)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                Attribute attribute = attributes[i];
                Attribute convAttr = Conversion(attribute);
                if (convAttr == null)
                    continue;
                else
                {
                    attributes[i] = convAttr;
                }
            }
        }

        private static Attribute Conversion(Attribute attribute)
        {
            switch (attribute)
            {
                case MvcAttributes.AcceptVerbsAttribute attr:
                    return new AcceptVerbsAttribute(attr.HttpMethods.ToArray()) { Name = attr.Name, Order = attr.Order, Route = attr.Route };
                case MvcAttributes.AllowAnonymousAttribute attr:
                    return new AllowAnonymousAttribute();
                case MvcAttributes.AuthorizeAttribute attr:
                    return new AuthorizeAttribute(attr.Policy) { AuthenticationSchemes = attr.AuthenticationSchemes, Roles = attr.Roles };
                case MvcAttributes.FromBodyAttribute attr:
                    return new FromBodyAttribute();
                case MvcAttributes.FromFormAttribute attr:
                    return new FromFormAttribute() { Name = attr.Name };
                case MvcAttributes.FromHeaderAttribute attr:
                    return new FromHeaderAttribute() { Name = attr.Name };
                case MvcAttributes.FromQueryAttribute attr:
                    return new FromQueryAttribute() { Name = attr.Name };
                case MvcAttributes.FromRouteAttribute attr:
                    return new FromRouteAttribute() { Name = attr.Name };
                case MvcAttributes.FromServicesAttribute attr:
                    return new FromServicesAttribute();
                case MvcAttributes.HttpDeleteAttribute attr:
                    return new HttpDeleteAttribute(attr.Template) { Name = attr.Name, Order = attr.Order };
                case MvcAttributes.HttpGetAttribute attr:
                    return new HttpGetAttribute(attr.Template) { Name = attr.Name, Order = attr.Order };
                case MvcAttributes.HttpHeadAttribute attr:
                    return new HttpHeadAttribute(attr.Template) { Name = attr.Name, Order = attr.Order };
                case MvcAttributes.HttpOptionsAttribute attr:
                    return new HttpOptionsAttribute(attr.Template) { Name = attr.Name, Order = attr.Order };
                case MvcAttributes.HttpPatchAttribute attr:
                    return new HttpPatchAttribute(attr.Template) { Name = attr.Name, Order = attr.Order };
                case MvcAttributes.HttpPostAttribute attr:
                    return new HttpPostAttribute(attr.Template) { Name = attr.Name, Order = attr.Order };
                case MvcAttributes.HttpPutAttribute attr:
                    return new HttpPutAttribute(attr.Template) { Name = attr.Name, Order = attr.Order };
                case MvcAttributes.RouteAttribute attr:
                    return new RouteAttribute(attr.Template) { Name = attr.Name, Order = attr.Order };
                case MvcAttributes.NonActionAttribute attr:
                    return new NonActionAttribute();
                case MvcAttributes.NonControllerAttribute attr:
                    return new NonControllerAttribute();
               
                default:
                    return null;
            }

        }
    }
}
