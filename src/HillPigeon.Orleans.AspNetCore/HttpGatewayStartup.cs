using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Runtime;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HillPigeon.Orleans.AspNetCore
{
    public class HttpGatewayStartup : IStartupTask, IDisposable
    {
        private readonly ILogger<HttpGatewayStartup> _logger;
        private readonly HttpGatewayOptions _options;
        private IWebHost host;
        public HttpGatewayStartup(ILogger<HttpGatewayStartup> logger, IOptions<HttpGatewayOptions> options)
        {
            this._logger = logger;
            this._options = options.Value;
        }
        public void Dispose()
        {
            try
            {
                host?.Dispose();
            }
            catch
            {
                /* NOOP */
            }
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            try
            {
                host = new WebHostBuilder()
                        .ConfigureServices(services =>
                        {
                            var mvcBuild = services
                                  .AddMvcCore()
                                  .AddJsonFormatters();

                            this._options.MvcBuilderAction?.Invoke(mvcBuild);
                        })
                        .UseKestrel()
                        .UseUrls($"http://{_options.Host}:{_options.Port}")
                        .Build();

                await host.StartAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(10001, ex.ToString());
            }
        }
    }
}
