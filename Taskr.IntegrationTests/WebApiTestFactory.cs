using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Taskr.Api;
using Taskr.Persistance;
using WebMotions.Fake.Authentication.JwtBearer;

namespace Taskr.IntegrationTests
{
    public class WebApiTestFactory : WebApplicationFactory<Startup>
    {

      
        
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
        }
 
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");
            builder
                .UseTestServer()
                .ConfigureTestServices(collection =>
                {
                    collection.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                    }).AddFakeJwtBearer();
                });
            // builder.ConfigureServices(services =>
            // {
            //     services.RemoveAll(typeof(DataContext));
            //     services.AddDbContext<DataContext>(options =>
            //     {
            //         options.UseInMemoryDatabase("TestDb");
            //     });
            // });
            base.ConfigureWebHost(builder);
        }
        
        
    }
}