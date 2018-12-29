using System.Collections.Generic;
using System.Reflection;

namespace HillPigeon.ApplicationModels
{
    public class ControllerFeature
    {
        public ControllerFeature(string name)
        {
            this.ModuleName = name;
        }
        public string ModuleName { get; }
        public IList<TypeInfo> Controllers { get; } = new List<TypeInfo>();
    }
}
