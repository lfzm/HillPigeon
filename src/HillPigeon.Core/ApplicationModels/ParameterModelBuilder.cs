using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            parameterModel.ActionContext = actionModel;

            this.WithBindingSource(parameterModel, parameter);
            this.WithConvention(parameterModel);
            return parameterModel;
        }


        private void WithBindingSource(ParameterModel parameterModel, ParameterInfo parameterInfo)
        {
            this.WithBindingSource<FromBodyAttribute>(parameterModel, parameterInfo);
            this.WithBindingSource<FromFormAttribute>(parameterModel, parameterInfo);
            this.WithBindingSource<FromHeaderAttribute>(parameterModel, parameterInfo);
            this.WithBindingSource<FromQueryAttribute>(parameterModel, parameterInfo);
            this.WithBindingSource<FromRouteAttribute>(parameterModel, parameterInfo);
            this.WithBindingSource<FromServicesAttribute>(parameterModel, parameterInfo);
        }

        private void WithBindingSource<TAttribute>(ParameterModel parameterModel, ParameterInfo parameterInfo)
            where TAttribute : Attribute, IBindingSourceMetadata
        {
            var attr = parameterInfo.GetCustomAttribute<TAttribute>();
            if (attr != null)
            {
                parameterModel.BindingSources.Add((IBindingSourceMetadata)attr);
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
