using HillPigeon.ApplicationBuilder;
using HillPigeon.ApplicationModels;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace HillPigeon
{
    internal class ServiceControllerFactory : IServiceControllerFactory
    {
        private readonly ApplicationModelFactory _applicationModelFactory;
        private readonly IServiceControllerBuilder _serviceControllerBuilder;
        public ServiceControllerFactory(ApplicationModelFactory applicationModelFactory, IServiceControllerBuilder serviceControllerBuilder)
        {
            this._applicationModelFactory = applicationModelFactory;
            this._serviceControllerBuilder = serviceControllerBuilder;
        }
        public IEnumerable<TypeInfo> Create()
        {
            List<Task<TypeInfo>> tasks = new List<Task<TypeInfo>>();
            IList<TypeInfo> services = new List<TypeInfo>();

            var applications = _applicationModelFactory.CreateApplications();
            foreach (var application in applications)
            {
                foreach (var controller in application.Controllers)
                {
                    var task = _serviceControllerBuilder.Build(new ServiceControllerBuildContext(controller));
                    tasks.Add(task);
                }
            }
            Task.WaitAll(tasks.ToArray());
            tasks.ForEach(f => services.Add(f.Result));
            _serviceControllerBuilder.Dispose();
            return services;
        }
    }
}
