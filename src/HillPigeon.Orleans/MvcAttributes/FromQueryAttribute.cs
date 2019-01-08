
using System;

namespace HillPigeon.MvcAttributes
{
    /// <summary>
    /// Specifies that a parameter or property should be bound using the request query string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FromQueryAttribute : Attribute
    {
        /// <inheritdoc />
        public string Name { get; set; }
    }
}
