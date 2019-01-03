using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HillPigeon.ApplicationModels
{
    public class ParameterModelBuilder
    {
        private readonly IParameterModelConvention[] _parameterModelConventions;
        public ParameterModelBuilder(IEnumerable<IParameterModelConvention> parameterModelConventions)
        {
            _parameterModelConventions = parameterModelConventions.ToArray();
        }
        public ParameterModel Build(ActionModel actionModel, ParameterInfo parameter)
        {
            ParameterModel parameterModel = new ParameterModel(parameter);
            parameterModel.ActionModel = actionModel;

            this.WithAttributes(parameterModel, parameter);
            this.WithConvention(parameterModel);
            return parameterModel;
        }
        private void WithAttributes(ParameterModel parameterModel, ParameterInfo parameterInfo)
        {
            var attributes = parameterInfo.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                parameterModel.Attributes.Add((Attribute)attribute);
            }
        }
        private void WithConvention(ParameterModel parameterModel)
        {
            foreach (var item in _parameterModelConventions)
            {
                item.Apply(parameterModel);
            }
        }
    }
}
