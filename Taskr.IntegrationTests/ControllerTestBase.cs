using System;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Taskr.Commands.Auth;
using Taskr.Domain;
using Taskr.Dtos.Auth;
using Xunit;

namespace Taskr.IntegrationTests
{
    public class ControllerTestBase : IClassFixture<WebApiTestFactory>
    {
        protected readonly WebApiTestFactory factory;
        protected HttpClient client;
        protected dynamic token;

        public ControllerTestBase(WebApiTestFactory factory)
        {
            this.factory = factory;
            client = factory.CreateClient();
            token = new ExpandoObject();
            token.sub = "TestUsername";
            token.email = "test@test.com";
            token.jti = Guid.NewGuid();
            token.id = Guid.NewGuid();
        }

        public async Task CreateUserAndAuthorizeAsync()
        {
            var response = await client.PostAsJsonAsync("api/v1/auth/signup", new SignUpCommand
            {
                Bio = "i am a test",
                City = "test city",
                Email = "integration@test.com",
                Password = "Pa$$w0rd",
                FirstName = "integration",
                LastName = "test",
                Username = "intTest"
            });

            var authResponse = await response.Content.ReadAsStringAsync();
            var validatedSignup =  JsonConvert.DeserializeObject<AuthSuccessResponse>(authResponse);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", validatedSignup.Token);
        }
        
    }
}