using HillPigeon.ApplicationModels;
using HillPigeon.ApplicationParts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace HillPigeon.Core.Test.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IApplicationFeatureProvider, TestApplicationFeatureProvider>();
            services.AddMvcCore()
                .AddJsonFormatters()
                .AddHillPigeon(build =>
                {
                    build.PartManager.AddApplicationPart(new AssemblyPart(this.GetType().Assembly));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
          
            services.AddScoped<TestService>();
            if (services.All(u => u.ServiceType != typeof(IHttpContextAccessor)))
            {
                services.AddScoped(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
