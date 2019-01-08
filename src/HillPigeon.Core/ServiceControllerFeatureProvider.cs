using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HillPigeon
{
    public class ServiceControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly Func<IServiceProvider> _serviceProviderFactory;
        private IServiceProvider _serviceProvider;
        private IServiceControllerFactory _serviceControllerFactory;
        public ServiceControllerFeatureProvider(Func<IServiceProvider> serviceProviderFactory)
        {
            this._serviceProviderFactory = serviceProviderFactory;
        }
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = _serviceProviderFactory.Invoke();
                _serviceControllerFactory = _serviceProvider.GetRequiredService<IServiceControllerFactory>();
            }
            var services = this._serviceControllerFactory.Create();
            foreach (var service in services)
            {
                feature.Controllers.Add(service);
            }
        }

    }
}
