
using System;

namespace HillPigeon.MvcAttributes
{
    /// <summary>
    /// Indicates that a controller method is not an action method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class NonActionAttribute : Attribute
    {
    }
}