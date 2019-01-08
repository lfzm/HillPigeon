
using System;

namespace HillPigeon.MvcAttributes
{
    /// <summary>
    /// Specifies that a parameter or property should be bound using the request headers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FromHeaderAttribute : Attribute
    {
        /// <inheritdoc />
        public string Name { get; set; }
    }
}
