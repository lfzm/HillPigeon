using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HillPigeon.ApplicationBuilder
{
    public static class CustomAttributeBuilderFactory
    {
        public static CustomAttributeBuilder Build(Attribute attribute)
        {
            ConstructorInfo constructorInfo = null;
            object[] values = null;
            if (attribute is AuthorizeAttribute || attribute is AllowAnonymousAttribute)
            {
                //无需赋值
            }
            else if (attribute is ApiVersionAttribute apiVerAttr)
            {
                constructorInfo = attribute.GetType().GetConstructor(new Type[] { typeof(ApiVersion) });
                foreach (var versions in apiVerAttr.Versions)
                {
                    values = new object[] { versions };
                    return Build(attribute, constructorInfo, values);
                }
            }
            else if (attribute is RouteAttribute routeAttr)
            {
                constructorInfo = attribute.GetType().GetConstructor(new Type[] { typeof(string) });
                values = new object[] { routeAttr.Template };
            }
            else if (attribute is AcceptVerbsAttribute acceptVerbs)
            {
                constructorInfo = attribute.GetType().GetConstructor(new Type[] { typeof(string[]) });
                values = new object[] { acceptVerbs.HttpMethods.ToArray() };
            }
            else if (attribute is HttpDeleteAttribute || attribute is HttpGetAttribute || attribute is HttpHeadAttribute || attribute is HttpPatchAttribute || attribute is HttpOptionsAttribute || attribute is HttpPostAttribute || attribute is HttpPutAttribute)
            {
                var httpMethod = (HttpMethodAttribute)attribute;
                var _acceptVerbs = new AcceptVerbsAttribute(httpMethod.HttpMethods.ToArray())
                {
                    Name = httpMethod.Name,
                    Order = httpMethod.Order,
                    Route = httpMethod.Template
                };
                constructorInfo = _acceptVerbs.GetType().GetConstructor(new Type[] { typeof(string[]) });
                values = new object[] { _acceptVerbs.HttpMethods.ToArray() };
                return Build(_acceptVerbs, constructorInfo, values);
            }
            else if (attribute is FromBodyAttribute || attribute is FromFormAttribute || attribute is FromHeaderAttribute || attribute is FromQueryAttribute || attribute is FromRouteAttribute || attribute is FromServicesAttribute)
            {
                // 无需赋值
            }
            else
            {
                throw new NotSupportedException(nameof(attribute));
            }
            return Build(attribute, constructorInfo, values);
        }
        public static CustomAttributeBuilder Build(Attribute attribute, ConstructorInfo con, object[] constructorArgs)
        {
            var type = attribute.GetType();
            if (con == null)
            {
                con = type.GetConstructor(new Type[0]);
                constructorArgs = new object[0];
            }
            var properties = attribute.GetType().GetProperties().Where(f => f.CanWrite && f.CanRead).ToArray();
            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                values[i] = property.GetValue(attribute);
            }
            return new CustomAttributeBuilder(con, constructorArgs, properties, values);
        }
    }
}
