using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

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
                if (this.IsController(service))
                {
                    feature.Controllers.Add(service);
                }
            }
        }
        protected bool IsController(Type typeInfo)
        {
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

            //if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase) &&
            //    !typeInfo.IsDefined(typeof(ControllerAttribute)))
            //{
            //    return false;
            //}

            return true;
        }
    }
}
