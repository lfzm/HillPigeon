using System;
using System.Collections.Concurrent;
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
            var type = this
                .BuildModule(context.Controller.ModuleName) //1.构建程序集、创建模块
                .BuildType(context, this.BuildControllerIL);//3.定义类
            context.DynamicTypeInfo = type;
            return Task.FromResult(type);
        }
        private void BuildControllerIL(TypeBuilder builder, ServiceControllerBuildContext context)
        {
            context.ServiceProviderField = builder.DefineField("_serviceProvider", typeof(IServiceProvider), FieldAttributes.InitOnly | FieldAttributes.Public);
            var controllerModel = context.Controller;
            builder
                .BuildRouteAttributes(controllerModel.Routes)
                .BuildAuthorizeAttributes(controllerModel.Authorizes)
                .BuildConstructor(il =>
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Stfld, context.ServiceProviderField);
                    il.Emit(OpCodes.Ret);
                });

            foreach (var acion in controllerModel.Actions)
            {
                _serviceActionBuilder.Build(builder, new ServiceActionBuildContext(acion, context));
            }
        }
        private ModuleBuilder BuildModule(string moduleName)
        {
           return  moduleBuilders.GetOrAdd(moduleName, (key) =>
            {
                string name = "HillPigeon.Core.DynamicClient";
                if (!string.IsNullOrEmpty(key))
                {
                    name = name + "."+ moduleName;
                }
                var asmName = new AssemblyName(name);
                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
                return  assemblyBuilder.DefineDynamicModule(name);
            });
          
        }

        public void Dispose()
        {
            this.moduleBuilders.Clear();
        }
    }
}
