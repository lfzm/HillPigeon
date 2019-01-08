
using System;

namespace HillPigeon.MvcAttributes
{
    /// <summary>
    /// Specifies that a parameter or property should be bound using route-data from the current request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FromRouteAttribute : Attribute
    {
        /// <inheritdoc />
        public string Name { get; set; }
    }
}
