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
            var builder = WebHost.CreateDefaultBuilder().UseStartup<Startup>();
            this.Server = new TestServer(builder);
        }
        public TestServer Server { get; }
    }
}
