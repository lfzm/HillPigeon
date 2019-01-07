using System;

namespace HillPigeon.Orleans
{
    /// <summary>
    /// Grain is not a primary key  attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class NotPrimaryKeyAttribute : Attribute
    {
    }
}
