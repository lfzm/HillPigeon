using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace HillPigeon
{
    public class ParameterModel
    {
        public ParameterModel(ParameterInfo parameterInfo)
        {
            this.Position = parameterInfo.Position+1;
            this.ParameterName = parameterInfo.Name;
            this.ParameterType = parameterInfo.ParameterType;
            this.ParameterAttributes = parameterInfo.Attributes;
            this.HasDefaultValue = parameterInfo.HasDefaultValue;
            this.DefaultValue = parameterInfo.DefaultValue;
            this.Attributes = new List<Attribute>();
        }
        public int Position { get; set; }
        public string ParameterName { get; set; }
        public Type ParameterType { get; set; }
        public ParameterAttributes ParameterAttributes { get; set; }
        public bool HasDefaultValue { get; set; }
        public object DefaultValue { get; set; }
        public IList<Attribute> Attributes { get; }
        public ActionModel ActionContext { get; set; }
    }
}
