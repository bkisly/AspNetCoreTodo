using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreTodo.IntegrationTests
{
    public class TestFixture : IDisposable
    {
        private readonly TestServer _server;
        public HttpClient Client { get; }

        public TestFixture()
        {
            var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\AspNetCoreTodo"));
                    config.AddJsonFile("appsettings.json");
                });
            });

            _server = application.Server;

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("https://localhost:7245");
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
