using HillPigeon.ApplicationBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HillPigeon.ApplicationModels
{
    public class ActionModelBuilder
    {
        private readonly ParameterModelBuilder _parameterModelBuilder;
        private readonly IActionModelConvention[] _actionModelConventions;
        public ActionModelBuilder(ParameterModelBuilder parameterModelBuilder, IEnumerable<IActionModelConvention> actionModelConventions)
        {
            _parameterModelBuilder = parameterModelBuilder;
            _actionModelConventions = actionModelConventions.ToArray();
        }
        public ActionModel Build(ControllerModel controller, MethodInfo methodInfo)
        {
            ActionModel actionModel = new ActionModel(methodInfo);
            actionModel.Controller = controller;

            this.WithActionName(actionModel, methodInfo);
            this.WithAttributes(actionModel, methodInfo);
            this.WithParameter(actionModel, methodInfo);
            this.WithActionIL(actionModel, methodInfo);
            this.WithConvention(actionModel);
            return actionModel;
        }
        private void WithActionName(ActionModel actionModel, MethodInfo methodInfo)
        {
            var actionName = methodInfo.GetCustomAttribute<ActionNameAttribute>(inherit: true);
            if (actionName?.Name != null)
            {
                actionModel.ActionName = actionName.Name;
            }
            else
            {
                actionModel.ActionName = methodInfo.Name;
            }
        }
        private void WithAttributes(ActionModel actionModel, MethodInfo methodInfo)
        {
            var attributes = methodInfo.GetCustomAttributes(inherit: true);
            foreach (var attribute in attributes)
            {
                actionModel.Attributes.Add((Attribute)attribute);
            }
        }
        private void WithParameter(ActionModel actionModel, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            foreach (var parameter in parameters)
            {
                var parameterModel = _parameterModelBuilder.Build(actionModel, parameter);
                actionModel.Parameters.Add(parameterModel);
            }
        }
        private void WithActionIL(ActionModel actionModel, MethodInfo methodInfo)
        {
            actionModel.GeneratActionIL = (ServiceActionBuildContext context, ILGenerator il) =>
            {
                ActionModel action = context.Action;
                //定义二个存储
                il.DeclareLocal(action.Controller.ControllerType);
                il.DeclareLocal(action.ReturnType);

                //获取控制器
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, context.ControllerBuildContext.ServiceProviderField);
                il.Emit(OpCodes.Call, typeof(ServiceProviderServiceExtensions).GetMethod("GetRequiredService", new Type[] { typeof(IServiceProvider) }).MakeGenericMethod(action.Controller.ControllerType));
                il.Emit(OpCodes.Stloc_0);

                //调用方法
                il.Emit(OpCodes.Ldloc_0);
                for (int i = 1; i <= actionModel.Parameters.Count; i++)
                {
                    il.Emit(OpCodes.Ldarg_S, i);
                }
                il.Emit(OpCodes.Callvirt, action.MethodInfo);
                il.Emit(OpCodes.Stloc_1);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Ret);
            };
        }
        private void WithConvention(ActionModel actionModel)
        {
            foreach (var item in _actionModelConventions)
            {
                item.Apply(actionModel);
            }
        }
    }
}
