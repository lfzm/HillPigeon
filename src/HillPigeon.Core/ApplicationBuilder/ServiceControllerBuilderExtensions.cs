using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HillPigeon.ApplicationBuilder
{
    public static class ServiceControllerBuilderExtensions
    {
        /// <summary>
        /// 组装<see cref=" ModuleBuilder"/>
        /// </summary>
        /// <param name="builder"><see cref="AssemblyBuilder"/></param>
        /// <param name="name">模块名称</param>
        /// <returns></returns>
        public static ModuleBuilder BuildModule(this AssemblyBuilder builder, string name)
        {
            string moduleName = "HillPigeon.Core.DynamicClient";
            if (!string.IsNullOrEmpty(name))
            {
                moduleName = moduleName + "." + name;
            }

            var module = builder.GetDynamicModule(moduleName);
            if (module != null)
                return module;
            return builder.DefineDynamicModule(moduleName);
        }
        public static TypeInfo BuildType(this ModuleBuilder builder, ServiceControllerBuildContext context, Action<TypeBuilder, ServiceControllerBuildContext> action)
        {
            var typeBuilder = builder.DefineType(context.Controller.ControllerName, TypeAttributes.Public);
            action.Invoke(typeBuilder, context);
            return typeBuilder.CreateTypeInfo();
        }
        public static TypeBuilder BuildField<T>(this TypeBuilder builder, string name, FieldAttributes attributes)
        {
            builder.DefineField(name, typeof(T), attributes);
            return builder;
        }

        public static TypeBuilder BuildMethod(this TypeBuilder builder, ServiceActionBuildContext context, Action<MethodBuilder, ServiceActionBuildContext> action)
        {
            var paramTypes = context.Action.Parameters.OrderBy(f => f.Position).Select(f => f.ParameterType).ToArray();
            var methodBuilder = builder.DefineMethod(context.Action.ActionName, MethodAttributes.Public, context.Action.ReturnType, paramTypes);
            action.Invoke(methodBuilder, context);
            return builder;
        }
        public static TypeBuilder BuildConstructor(this TypeBuilder builder, Action<ILGenerator> action)
        {
            var constructorBuild = builder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.HasThis, new Type[] { typeof(IServiceProvider) });
            var il = constructorBuild.GetILGenerator();
            action.Invoke(il);
            return builder;
        }
        public static TypeBuilder BuildRouteAttributes(this TypeBuilder builder, IEnumerable<RouteAttribute> routeAttrs)
        {
            var type = typeof(RouteAttribute);
            var properties = type.GetProperties().Where(f=>f.CanWrite &&f.CanRead).ToArray();
            foreach (var routeAttr in routeAttrs)
            {
                var con = type.GetConstructor(new Type[] { typeof(string) });
                var argr = new object[] { routeAttr.Template };
                var customAttributeBuilder = routeAttr.BuildCustomAttribute(con, argr, properties);
                builder.SetCustomAttribute(customAttributeBuilder);
            }
            return builder;
        }
        public static TypeBuilder BuildAuthorizeAttributes(this TypeBuilder builder, IEnumerable<AuthorizeAttribute> authAttrs)
        {
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
   
        public static CustomAttributeBuilder BuildCustomAttribute(this Attribute attr, ConstructorInfo con, object[] constructorArgs, FieldInfo[] fields)
        {
            object[] values = new object[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                values[i] = field.GetValue(attr);
            }

            return new CustomAttributeBuilder(con, constructorArgs, fields, values);
        }
        public static CustomAttributeBuilder BuildCustomAttribute(this Attribute attr, ConstructorInfo con, object[] constructorArgs, PropertyInfo[] propertys)
        {
            object[] values = new object[propertys.Length];
            for (int i = 0; i < propertys.Length; i++)
            {
                PropertyInfo property = propertys[i];
                values[i] = property.GetValue(attr);
            }
            return new CustomAttributeBuilder(con, constructorArgs, propertys, values);
        }
    }
}
