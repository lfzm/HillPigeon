using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.Orleans
{
    public static class OrleansGrainHelper
    {
        public static bool IsGrain(Type type)
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
