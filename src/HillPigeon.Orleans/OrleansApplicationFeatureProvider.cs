using HillPigeon.ApplicationModels;
using HillPigeon.ApplicationParts;
using Orleans;
using System;

namespace HillPigeon.Orleans
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
                if (OrleansGrainHelper.IsGrain(type))
                {
                    feature.Controllers.Add(type);
                }
            }
        }

       
    }
}
