using System.Collections.Generic;
using System.Reflection;

namespace HillPigeon.ApplicationParts
{
    /// <summary>
    /// A part of an MVC application.
    /// </summary>
    public abstract class ApplicationPart
    {
        public abstract string ModuleName { get; }
        /// <summary>
        /// Gets the list of available types in the <see cref="ApplicationPart"/>.
        /// </summary>
        public abstract IEnumerable<TypeInfo> Types { get; }
        /// <summary>
        /// Gets the <see cref="ApplicationPart"/> name.
        /// </summary>
        public abstract string AssemblyName { get; }
  
    }
}
