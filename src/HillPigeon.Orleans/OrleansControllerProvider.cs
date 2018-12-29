using HillPigeon.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.AspNetCore.Orleans
{
    public class OrleansControllerProvider
    {
        //private readonly IControllerModelFactory controllerModelBuilder;
        //private readonly IActionModelFactory actionModelBuilder;
        //private readonly IParameterModelFactory parameterModelBuilder;

        //public IEnumerable<ControllerModel> GetControllers()
        //{
        //    throw new NotImplementedException();
        //}




        //public void BuildMethodIL(ActionModel context, ILGenerator il)
        //{
        //    var primaryKeyParam = context.Parameters.Where(f => f.ParameterName == "__primaryKey").FirstOrDefault();
        //    var keyExtensionParam = context.Parameters.Where(f => f.ParameterName == "__keyExtension").FirstOrDefault();
        //    var grainClassParam = context.Parameters.Where(f => f.ParameterName == "__grainClassNamePrefix").FirstOrDefault();

        //    //定义四个存储
        //    il.DeclareLocal(primaryKeyParam.ParameterType);
        //    il.DeclareLocal(typeof(IOrleansClient));
        //    il.DeclareLocal(context.Controller.ControllerType);
        //    il.DeclareLocal(context.ReturnType);

        //    //设置PrimaryKey
        //    il.Emit(OpCodes.Ldarg_S, primaryKeyParam.Position);
        //    il.Emit(OpCodes.Stloc_0);

        //    //获取IOrleansClient
        //    il.Emit(OpCodes.Ldarg_0);
        //    il.Emit(OpCodes.Ldfld, context.ControllerModel.ServiceProviderField);
        //    il.Emit(OpCodes.Call, typeof(ServiceProviderServiceExtensions).GetMethod("GetRequiredService", new Type[] { typeof(IServiceProvider) }).MakeGenericMethod(typeof(IOrleansClient)));
        //    il.Emit(OpCodes.Stloc_1);

        //    //获取服务接口
        //    il.Emit(OpCodes.Ldloc_1);
        //    il.Emit(OpCodes.Ldloc_0);//primaryKey 
        //    il.Emit(OpCodes.Ldarg_S, keyExtensionParam.Position);//keyExtension
        //    il.Emit(OpCodes.Ldarg_S, grainClassParam.Position);//grainClassNamePrefix
        //    il.Emit(OpCodes.Callvirt, typeof(IOrleansClient).GetMethod("GetGrain", new Type[] { primaryKeyParam.ParameterType, keyExtensionParam.ParameterType, grainClassParam.ParameterType }).MakeGenericMethod(context.ControllerModel.InterfaceType));
        //    il.Emit(OpCodes.Stloc_2);

        //    //调用接口
        //    il.Emit(OpCodes.Ldloc_2);
        //    foreach (var param in context.Parameters.OrderBy(f => f.Position))
        //    {
        //        if (param.ParameterName == "__primaryKey" ||
        //            param.ParameterName == "__keyExtension" ||
        //            param.ParameterName == "__grainClassNamePrefix")
        //            continue;

        //        il.Emit(OpCodes.Ldarg_S, param.Position);
        //    }
        //    il.Emit(OpCodes.Callvirt, context.MethodInfo);
        //    il.Emit(OpCodes.Stloc_3);
        //    il.Emit(OpCodes.Ldloc_3);
        //    il.Emit(OpCodes.Ret);
        //}
    }
}
