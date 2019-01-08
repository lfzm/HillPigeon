using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IO;

namespace HillPigeon.Core.Test.Infrastructure
{
    public class WebHostTest
    {
        public WebHostTest()
        {
            try
            {
                var builder = new WebHostBuilder().UseStartup<Startup>();
                this.Server = new TestServer(builder);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        public TestServer Server { get; }
    }
}
