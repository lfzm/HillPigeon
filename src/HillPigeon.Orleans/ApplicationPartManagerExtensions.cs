using Orleans;
using Orleans.ApplicationParts;
using Orleans.Hosting;
using Orleans.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HillPigeon.Orleans
{
    public static class ApplicationPartManagerExtensions
    {

        public static void Get(this IServiceProvider serviceProvider)
        {
            var context = (HostBuilderContext)serviceProvider.GetService(typeof(HostBuilderContext));
            object key = context.Properties.Keys.Where(f => f.GetType() == typeof(object)).FirstOrDefault();
            var partManager = (IApplicationPartManager)context.Properties[key];
            var grainInterfaceFeature = partManager.CreateAndPopulateFeature<GrainInterfaceFeature>();
            var grainClassFeature = partManager.CreateAndPopulateFeature<GrainClassFeature>();

            var keyList = new List<string>() { "Orleans.Core.dll", "Orleans.Core.Abstractions.dll", "Orleans.Runtime.dll" };
            Dictionary<string, Assembly> assemblys = new Dictionary<string, Assembly>();
            foreach (var inter in grainInterfaceFeature.Interfaces)
            {
                string name = inter.InterfaceType.Assembly.ManifestModule.Name;
                if (keyList.Contains(name))
                    continue;

                if (!assemblys.ContainsKey(name))
                {
                    assemblys.Add(name, inter.InterfaceType.Assembly);
                }
            }
        }
    }
}
