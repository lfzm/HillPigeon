using HillPigeon.ApplicationBuilder;
using HillPigeon.ApplicationModels;
using HillPigeon.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HillPigeon.Orleans
{
    public class OrleansParameterModelConvention : IParameterModelConvention
    {
        public void Apply(ParameterModel parameterModel)
        {
            //判断是否需要添加 IBindingSourceMetadata  Attribute
            if (parameterModel.Attributes.Where(attr => attr.IsBindingSourceAttribute()).Count() > 0)
                return;

            //if the type can have children ,the use FromBodyAttribute 
            if (parameterModel.ParameterType.CanHaveChildren())
            {
                parameterModel.Attributes.Add(new FromBodyAttribute());
            }
            else
            {
                parameterModel.Attributes.Add(new FromQueryAttribute());
            }
        }
    }
}
