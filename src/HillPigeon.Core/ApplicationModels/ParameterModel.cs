using System;
using System.Collections.Generic;
using System.Reflection;

namespace HillPigeon
{
    public class ParameterModel
    {
        public ParameterModel()
        {
            this.Attributes = new List<Attribute>();

        }
        public ParameterModel(ParameterInfo parameterInfo) : this()
        {
            this.Position = parameterInfo.Position + 1;
            this.ParameterName = parameterInfo.Name;
            this.ParameterType = parameterInfo.ParameterType;
            this.ParameterAttributes = parameterInfo.Attributes;
            this.HasDefaultValue = parameterInfo.HasDefaultValue;
            this.DefaultValue = parameterInfo.DefaultValue;
        }
        public int Position { get; set; }
        public string ParameterName { get; set; }
        public Type ParameterType { get; set; }
        public ParameterAttributes ParameterAttributes { get; set; }
        public bool HasDefaultValue { get; set; }
        public object DefaultValue { get; set; }
        public IList<Attribute> Attributes { get; }
        public ActionModel ActionModel { get; set; }

        public string Feature { get; set; }
    }
}
