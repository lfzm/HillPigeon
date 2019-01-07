using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HillPigeon
{
    public class ServiceControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceControllerFactory _serviceControllerFactory;
        public ServiceControllerFeatureProvider(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._serviceControllerFactory = serviceProvider.GetRequiredService<IServiceControllerFactory>();
        }
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var services = this._serviceControllerFactory.Create();
            foreach (var service in services)
            {
                feature.Controllers.Add(service);
            }
        }
  
    }
}
