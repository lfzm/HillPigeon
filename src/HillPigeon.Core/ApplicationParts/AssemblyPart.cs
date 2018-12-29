using System;
using System.Collections.Generic;
using System.Reflection;

namespace HillPigeon.ApplicationParts
{
    public class AssemblyPart : ApplicationPart
    {
        /// <summary>
        /// Initializes a new <see cref="AssemblyPart"/> instance.
        /// </summary>
        /// <param name="assembly">The backing <see cref="System.Reflection.Assembly"/>.</param>
        /// <param name="name">appclition bame</param>
        public AssemblyPart(Assembly assembly, string name = "")
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            ModuleName = name;
        }
        /// <summary>
        /// Gets the <see cref="Assembly"/> of the <see cref="ApplicationPart"/>.
        /// </summary>
        public Assembly Assembly { get; }
        public override string AssemblyName  => Assembly.GetName().Name;
        public override IEnumerable<TypeInfo> Types => Assembly.DefinedTypes;
        public override string ModuleName { get; }
    }
}
