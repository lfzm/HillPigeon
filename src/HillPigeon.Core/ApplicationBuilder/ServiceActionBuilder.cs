using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HillPigeon.ApplicationBuilder
{
    internal class ServiceActionBuilder : IServiceActionBuilder
    {
        public void Build(TypeBuilder builder, ServiceActionBuildContext context)
        {
            var paramTypes = context.Action.Parameters.OrderBy(f => f.Position).Select(f => f.ParameterType).ToArray();
            var methodBuilder = builder.DefineMethod(context.Action.ActionName, MethodAttributes.Public, context.Action.ReturnType, paramTypes);

            this.BuildAction(methodBuilder, context);
        }
        public void BuildAction(MethodBuilder builder, ServiceActionBuildContext context)
        {
            var actionModel = context.Action;
            this.BuildParameter(builder, actionModel);
            this.BuildAttribute(builder, actionModel.Attributes);
            this.BuildMethodContext(builder, context, actionModel.GeneratActionIL);
        }

        public MethodBuilder BuildParameter(MethodBuilder builder, ActionModel context)
        {
            foreach (var param in context.Parameters)
            {
                this.BuildParameter(builder, param);
            }
            return builder;
        }
        public MethodBuilder BuildParameter(MethodBuilder builder, ParameterModel context)
        {
            var paramBuilder = builder.DefineParameter(context.Position, context.ParameterAttributes, context.ParameterName);
            if (context.HasDefaultValue)
            {
                paramBuilder.SetConstant(context.DefaultValue);
            }
            foreach (var attr in context.Attributes)
            {
                var customAttributeBuilder = CustomAttributeBuilderFactory.Build(attr);
                if (customAttributeBuilder != null)
                    paramBuilder.SetCustomAttribute(customAttributeBuilder);
            }
            return builder;
        }
        public MethodBuilder BuildAttribute(MethodBuilder builder, IList<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                var customAttributeBuilder = CustomAttributeBuilderFactory.Build(attribute);
                if (customAttributeBuilder != null)
                    builder.SetCustomAttribute(customAttributeBuilder);
            }
            return builder;
        }

        public MethodBuilder BuildMethodContext(MethodBuilder builder, ServiceActionBuildContext context, Action<ServiceActionBuildContext, ILGenerator> action)
        {
            var il = builder.GetILGenerator();
            action.Invoke(context, il);
            return builder;
        }
    }
}
