using System;
using System.Dynamic;
using System.Net.Http;
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
    }
}