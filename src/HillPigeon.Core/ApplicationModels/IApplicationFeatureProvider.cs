using HillPigeon.ApplicationParts;
using System.Collections.Generic;

namespace HillPigeon.ApplicationModels
{
    public interface IApplicationFeatureProvider
    {
        void PopulateFeature(ApplicationPart parts, ControllerFeature feature);
    }
}
