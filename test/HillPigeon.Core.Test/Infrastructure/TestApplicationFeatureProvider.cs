using HillPigeon.ApplicationModels;
using HillPigeon.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HillPigeon.Core.Test.Infrastructure
{
    public class TestApplicationFeatureProvider : IApplicationFeatureProvider
    {
        public void PopulateFeature(ApplicationPart parts, ControllerFeature feature)
        {
            var type = parts.Types.Where(f => f.Name == nameof(TestService)).FirstOrDefault();
            feature.Controllers.Add(type);
        }
    }
}
