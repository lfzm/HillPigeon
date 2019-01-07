using HillPigeon.ApplicationModels;
using HillPigeon.ApplicationParts;
using Orleans;
using System;

namespace HillPigeon.Orleans.Core
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
                if (type.IsGrain())
                {
                    feature.Controllers.Add(type);
                }
            }
        }

       
    }
}
