using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HillPigeon.ApplicationBuilder
{
    public static class ServiceActionBuilderExtensions
    {
        public static MethodBuilder BuildParameter(this MethodBuilder builder, ActionModel context)
        {
            foreach (var param in context.Parameters)
            {
                builder.BuildParameter(param);
            }
            return builder;
        }
        public static MethodBuilder BuildParameter(this MethodBuilder builder, ParameterModel context)
        {
            var paramBuilder = builder.DefineParameter(context.Position, context.Attributes, context.ParameterName);
            if (context.HasDefaultValue)
            {
                paramBuilder.SetConstant(context.DefaultValue);
            }
            foreach (var bs in context.BindingSources)
            {
                var type = bs.GetType();
                if (!typeof(Attribute).IsAssignableFrom(type))
                    continue;

                var properties = type.GetProperties().Where(f => f.CanWrite && f.CanRead).ToArray();
                var con = type.GetConstructor(new Type[0]);

                var customAttributeBuilder = ((Attribute)bs).BuildCustomAttribute(con, new object[0], properties);
                paramBuilder.SetCustomAttribute(customAttributeBuilder);
            }
            return builder;
        }
        public static MethodBuilder BuildRouteAttributes(this MethodBuilder builder, IEnumerable<RouteAttribute> routeAttrs)
        {
            var type = typeof(RouteAttribute);
            var properties = type.GetProperties().Where(f => f.CanWrite && f.CanRead).ToArray();
            foreach (var routeAttr in routeAttrs)
            {
                var con = type.GetConstructor(new Type[] { typeof(string) });
                var argr = new object[] { routeAttr.Template };
                var customAttributeBuilder = routeAttr.BuildCustomAttribute(con, argr, properties);
                builder.SetCustomAttribute(customAttributeBuilder);
            }
            return builder;
        }
        public static MethodBuilder BuildAuthorizeAttributes(this MethodBuilder builder, IEnumerable<AuthorizeAttribute> authAttrs, bool AllowAnonymous)
        {
            if (AllowAnonymous)
            {
                var customAttributeBuilder = new CustomAttributeBuilder(typeof(AllowAnonymousAttribute).GetConstructor(new Type[0]), new object[0]);
                builder.SetCustomAttribute(customAttributeBuilder);
                return builder;
            }
            if (authAttrs.Count() == 0)
                return builder;

            var type = typeof(AuthorizeAttribute);
            var properties = type.GetProperties().Where(f => f.CanWrite && f.CanRead).ToArray();
            foreach (var authAttr in authAttrs)
            {
                var con = type.GetConstructor(new Type[0]);
                var customAttributeBuilder = authAttr.BuildCustomAttribute(con, new object[0], properties);
                builder.SetCustomAttribute(customAttributeBuilder);
            }
            return builder;
        }
        public static MethodBuilder BuildHttpMethodAttributes(this MethodBuilder builder,  IEnumerable<AcceptVerbsAttribute> httpMethods)
        {
            foreach (var method in httpMethods)
            {
                var type = method.GetType();
                var properties = type.GetProperties().Where(f => f.CanWrite && f.CanRead).ToArray();
             
                var con = type.GetConstructor(new Type[] { typeof(string[]) });
                var vals = new object[] { method.HttpMethods.ToArray() };
                var customAttributeBuilder = method.BuildCustomAttribute(con, vals, properties);
                builder.SetCustomAttribute(customAttributeBuilder);
            }
            return builder;
        }
        public static MethodBuilder BuildMethodContext(this MethodBuilder builder, ServiceActionBuildContext context, Action<ServiceActionBuildContext, ILGenerator> action)
        {
            var il = builder.GetILGenerator();
            action.Invoke(context, il);
            return builder;
        }
    }
}
