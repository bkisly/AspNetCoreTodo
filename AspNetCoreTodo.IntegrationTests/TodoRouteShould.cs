using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net;

namespace AspNetCoreTodo.IntegrationTests
{
    public class TodoRouteShould : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;

        public TodoRouteShould(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task ChallengeAnnonymousUser()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/todo");
            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            if(response.Headers.Location is not null)
                Assert.Equal("https://localhost:7245/Identity/Account/Login?ReturnUrl=%2Ftodo", response.Headers.Location.ToString());
        }
    }
}
