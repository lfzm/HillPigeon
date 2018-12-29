using HillPigeon.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.ApplicationModels
{
    internal class ApplicationModelFactory
    {
        private readonly IEnumerable<IApplicationModelProvider> _applicationModelProviders;
        public ApplicationModelFactory(IEnumerable<IApplicationModelProvider> applicationModelProviders)
        {
            this._applicationModelProviders = applicationModelProviders;
                 }
        public IEnumerable<ApplicationModel> CreateApplications()
        {
            List<ApplicationModel> applications = new List<ApplicationModel>();
            foreach (var provider in _applicationModelProviders)
            {
                var application = provider.GetApplication();
                applications.Add(application);
            }
            return applications;
        }

    
    }
}
