using HillPigeon.ApplicationParts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HillPigeon.ApplicationModels
{
    internal class DefaultApplicationModelProvider : IApplicationModelProvider
    {
        private readonly IApplicationFeatureProvider[] _applicationFeatureProviders;
        private readonly ApplicationPartManager _applicationPartManager;
        private readonly ControllerModelBuilder _controllerModelBuilder;
        private readonly ILogger _logger;
        private ApplicationModel applicationModel;
        public DefaultApplicationModelProvider(
            ApplicationPartManager applicationPartManager,
            ControllerModelBuilder controllerModelBuilder,
            IEnumerable<IApplicationFeatureProvider> applicationFeatureProviders,
            ILogger<DefaultApplicationModelProvider> logger)
        {
            this._applicationPartManager = applicationPartManager;
            this._controllerModelBuilder = controllerModelBuilder;
            this._applicationFeatureProviders = applicationFeatureProviders.ToArray();
            this._logger = logger;
        }
        public ApplicationModel GetApplication()
        {
            applicationModel = new ApplicationModel();
            var applicationParts = _applicationPartManager.ApplicationParts;
            foreach (var applicationPart in applicationParts)
            {
                ControllerFeature controllerFeature = new ControllerFeature(applicationPart.ModuleName);
                this.PopulateFeature(applicationPart, controllerFeature);
                this.CreateControllerModel(controllerFeature);
            }
            return applicationModel;
        }

        public void CreateControllerModel(ControllerFeature controllerFeature)
        {
            foreach (var typeInfo in controllerFeature.Controllers)
            {
                var controller = _controllerModelBuilder.Build(typeInfo, controllerFeature.ModuleName);
                if (controller != null)
                {
                    applicationModel.Controllers.Add(controller);
                }
            }
        }

        public void PopulateFeature(ApplicationPart applicationPart, ControllerFeature feature)
        {
            if (feature == null)
            {
                throw new ArgumentNullException(nameof(feature));
            }

            foreach (var provider in this._applicationFeatureProviders)
            {
                provider.PopulateFeature(applicationPart, feature);
            }
        }
    }
}
