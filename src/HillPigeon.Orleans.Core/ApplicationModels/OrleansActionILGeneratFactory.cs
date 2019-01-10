using HillPigeon.ApplicationBuilder;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System;
using System.Linq;
using System.Reflection.Emit;

namespace HillPigeon.Orleans.ApplicationModels
{
    public class OrleansActionILGeneratFactory
    {
        public void GeneratActionIL(ServiceActionBuildContext context, ILGenerator il)
        {
            var actionModel = context.ActionModel;
            (Type Type, object DefaultValue) grainPrimaryKey = this.GrainInterfaceToKeyType(actionModel.Controller.ControllerType);
            ParameterModel keyExtensionParam = actionModel.Parameters.FirstOrDefault(f => f.Feature == "Orleans_keyExtension");
            ParameterModel grainClassParam = actionModel.Parameters.FirstOrDefault(f => f.ParameterName == "Orleans_grainClassNamePrefix");
            ParameterModel primaryKeyParam = null;
            //1、定义四个存储
            il.DeclareLocal(grainPrimaryKey.Type);
            il.DeclareLocal(typeof(IOrleansClient));
            il.DeclareLocal(actionModel.Controller.ControllerType);
            il.DeclareLocal(actionModel.ReturnType);

            //2、获取Grain PrimaryKey
            if (grainPrimaryKey.Type == typeof(Guid))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else
            {
                primaryKeyParam = actionModel.Parameters.FirstOrDefault(f => f.Feature == "Orleans_Primary");
                if (primaryKeyParam != null)
                    il.Emit(OpCodes.Ldarg_S, primaryKeyParam.Position);
            }
            il.Emit(OpCodes.Stloc_0);

            //3、获取IOrleansClient
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, context.ControllerBuildContext.ServiceProviderField);
            il.Emit(OpCodes.Call, typeof(ServiceProviderServiceExtensions).GetMethod("GetRequiredService", new Type[] { typeof(IServiceProvider) }).MakeGenericMethod(typeof(IOrleansClient)));
            il.Emit(OpCodes.Stloc_1);


            //4、获取Grain接口实例
            string methodName; Type[] paramTypes; 
            il.Emit(OpCodes.Ldloc_1);
            if (primaryKeyParam != null)
            {
                il.Emit(OpCodes.Ldloc_0);//primaryKey 
                methodName = "GetGrain";
                paramTypes = new Type[] { grainPrimaryKey.Type, keyExtensionParam.ParameterType, grainClassParam.ParameterType };
            }
            else
            {
                methodName = "GetGrainAutoPrimaryKey";
                paramTypes = new Type[] { keyExtensionParam.ParameterType, grainClassParam.ParameterType };

            }
            if (keyExtensionParam != null)
            {
                il.Emit(OpCodes.Ldarg_S, keyExtensionParam.Position);//keyExtension
            }
            il.Emit(OpCodes.Ldarg_S, grainClassParam.Position);//grainClassNamePrefix
            il.Emit(OpCodes.Callvirt, typeof(IOrleansClient).GetMethod(methodName, paramTypes).MakeGenericMethod(actionModel.Controller.ControllerType));
            il.Emit(OpCodes.Stloc_2);

            //5、调用接口
            il.Emit(OpCodes.Ldloc_2);
            foreach (var param in actionModel.Parameters.OrderBy(f => f.Position))
            {
                //排除primaryKey、keyExtension、grainClassNamePrefix的参数
                if (param.Feature == "Orleans_keyExtension" ||
                    param.Feature == "Orleans_Primary" ||
                    param.Feature == "Orleans_grainClassNamePrefix")
                {
                    continue;
                }
                il.Emit(OpCodes.Ldarg_S, param.Position);
            }
            il.Emit(OpCodes.Callvirt, actionModel.MethodInfo);
            il.Emit(OpCodes.Stloc_3);
            il.Emit(OpCodes.Ldloc_3);
            il.Emit(OpCodes.Ret);

        }

        public (Type type, object defaultValue) GrainInterfaceToKeyType(Type type)
        {
            if (typeof(IGrainWithIntegerCompoundKey).IsAssignableFrom(type) ||
              typeof(IGrainWithIntegerKey).IsAssignableFrom(type))
            {
                return new ValueTuple<Type, object>(typeof(long), default(long));
            }
            else if (typeof(IGrainWithStringKey).IsAssignableFrom(type))
            {
                return new ValueTuple<Type, object>(typeof(string), default(string));
            }
            else
            {
                return new ValueTuple<Type, object>(typeof(Guid), null);
            }
        }
    }
}
