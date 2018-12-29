using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace HillPigeon.ApplicationBuilder
{
    internal class ServiceControllerBuilder : IServiceControllerBuilder
    {
        private readonly IServiceActionBuilder _serviceActionBuilder;
        private readonly ConcurrentDictionary<string, ModuleBuilder> moduleBuilders = new ConcurrentDictionary<string, ModuleBuilder>();
        private readonly object objlock = new object();
        public ServiceControllerBuilder(IServiceActionBuilder serviceActionBuilder)
        {
            this._serviceActionBuilder = serviceActionBuilder;
        }
        public Task<TypeInfo> Build(ServiceControllerBuildContext context)
        {

            var typeBuilder = this.BuildType(context);
            this.BuildAttribute(typeBuilder, context.Controller.Attributes);
            this.BuildControllerIL(typeBuilder, context);
            this.BuildActions(typeBuilder, context);

            context.DynamicTypeInfo = typeBuilder.CreateTypeInfo();
            return Task.FromResult(context.DynamicTypeInfo);
        }

        private ModuleBuilder BuildModule(string moduleName)
        {
            return moduleBuilders.GetOrAdd(moduleName, (key) =>
            {
                string name = "HillPigeon.Core.DynamicClient";
                if (!string.IsNullOrEmpty(key))
                {
                    name = name + "." + moduleName;
                }
                var asmName = new AssemblyName(name);
                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
                return assemblyBuilder.DefineDynamicModule(name);
            });
        }
        private void BuildControllerIL(TypeBuilder builder, ServiceControllerBuildContext context)
        {
            context.ServiceProviderField = builder.DefineField("_serviceProvider", typeof(IServiceProvider), FieldAttributes.InitOnly | FieldAttributes.Public);
            var controllerModel = context.Controller;

            this.BuildConstructor(builder, il =>
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, context.ServiceProviderField);
                il.Emit(OpCodes.Ret);
            });
        }
        public TypeBuilder BuildType(ServiceControllerBuildContext context)
        {
            var moduleBuilder = this.BuildModule(context.Controller.ModuleName); //1.构建程序集、创建模块
            return moduleBuilder.DefineType(context.Controller.ControllerName, TypeAttributes.Public);//定义类
        }
        public void BuildActions(TypeBuilder builder, ServiceControllerBuildContext context)
        {
            foreach (var actionModel in context.Controller.Actions)
            {
                _serviceActionBuilder.Build(builder, new ServiceActionBuildContext(actionModel, context));
            }
        }
        public void BuildAttribute(TypeBuilder builder, IList<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                var customAttributeBuilder = CustomAttributeBuilderFactory.Build(attribute);
                builder.SetCustomAttribute(customAttributeBuilder);
            }
        }
        public void BuildConstructor(TypeBuilder builder, Action<ILGenerator> action)
        {
            var constructorBuild = builder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.HasThis, new Type[] { typeof(IServiceProvider) });
            var il = constructorBuild.GetILGenerator();
            action.Invoke(il);
        }
        public void Dispose()
        {
            this.moduleBuilders.Clear();
        }
    }
}
