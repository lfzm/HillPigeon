using HillPigeon.ApplicationBuilder;
using HillPigeon.ApplicationModels;
using HillPigeon.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace HillPigeon.DependencyInjection
{
    public class HillPigeonBuilder : IHillPigeonBuilder
    {
        public IApplicationPartManager PartManager { get; }

        public IServiceCollection Services { get; }

        public HillPigeonBuilder(IServiceCollection services)
        {
            this.Services = services;
            this.PartManager = new ApplicationParts.ApplicationPartManager();
        }

        public void Build()
        {
            this.Services.AddSingleton((ApplicationParts.ApplicationPartManager)PartManager);
            this.AddApplicationModels();
            this.AddApplicationBuilder();
            this.AddMVCFeatureProviders();
        }

        public void AddMVCFeatureProviders()
        {
            var mvcPartManager = (Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager)Services.FirstOrDefault(f => f.ServiceType == typeof(Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager)).ImplementationInstance;
            mvcPartManager.FeatureProviders.Add(new ServiceControllerFeatureProvider(() =>
            {
                return Services.BuildServiceProvider();
            }));
        }

        public void AddApplicationModels()
        {
            Services.AddSingleton<ApplicationModelFactory>();
            Services.AddSingleton<ControllerModelBuilder>();
            Services.AddSingleton<ActionModelBuilder>();
            Services.AddSingleton<ParameterModelBuilder>();
            Services.AddSingleton<IApplicationModelProvider, DefaultApplicationModelProvider>();
        }
        public void AddApplicationBuilder()
        {
            Services.AddSingleton<IServiceControllerFactory, ServiceControllerFactory>();
            Services.AddSingleton<IServiceControllerBuilder, ServiceControllerBuilder>();
            Services.AddSingleton<IServiceActionBuilder, ServiceActionBuilder>();
        }
    }
}
