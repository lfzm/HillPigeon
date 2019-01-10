using HillPigeon.ApplicationModels;
using HillPigeon.ApplicationParts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace HillPigeon.Orleans.ApplicationModels
{
    public class OrleansApplicationFeatureProvider : IApplicationFeatureProvider
    {
        public void PopulateFeature(ApplicationPart parts, ControllerFeature feature)
        {
            if (parts.Types == null)
            {
                return;
            }
            foreach (var type in parts.Types)
            {
                if (type.IsGrain() && this.IsController(type))
                {
                    feature.Controllers.Add(type);
                }
            }
        }
        protected bool IsController(Type typeInfo)
        {
            if (!typeInfo.IsInterface)
            {
                return false;
            }
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            // We only consider public top-level classes as controllers. IsPublic returns false for nested
            // classes, regardless of visibility modifiers
            if (!typeInfo.IsPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }
            return true;
        }

    }
}
