using Orleans;
using System;

namespace HillPigeon.Orleans.Core
{
    public static class OrleansGrainExtensions
    {
        public static bool IsGrain(this Type type)
        {
            if (!type.IsInterface)
            {
                return false;
            }
            if (!type.IsPublic)
            {
                return false;
            }

            if (typeof(IGrainWithGuidCompoundKey).IsAssignableFrom(type) ||
                typeof(IGrainWithGuidKey).IsAssignableFrom(type) ||
                typeof(IGrainWithIntegerCompoundKey).IsAssignableFrom(type) ||
                typeof(IGrainWithIntegerKey).IsAssignableFrom(type) ||
                typeof(IGrainWithStringKey).IsAssignableFrom(type))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
