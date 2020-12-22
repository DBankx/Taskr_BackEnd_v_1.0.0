using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Taskr.Domain;
using Xunit;

namespace Taskr.IntegrationTests
{
    public class JobControllerTests : ControllerTestBase
    {
        public JobControllerTests(WebApiTestFactory factory) : base(factory)
        {
        }
        
        [Fact]
        public async Task Should_return_All_Tasks_with_ok_statusCode()
        {
            client.SetFakeBearerToken((object) token);
            
            var response = await client.GetAsync("/api/v1/jobs");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Domain.Job>>(json);
            data.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetTaskById_Should_return_NotFound_with_incorrect_id()
        {
            // Arrange
            client.SetFakeBearerToken((object) token);
            var taskId = Guid.NewGuid();
            
            //Act
            var response = await client.GetAsync($"/api/v1/jobs/{taskId}");
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Creates new jobs in the database")]
        public async Task CreateTask_Should_create_a_new_Task()
        {
            // Arrange
            client.SetFakeBearerToken((object) token);
            var job = new Job
            {
                Title = "New test job",
                Description = "Test code for your new job",
                InitialPrice = 200000
            };
            
            // Act
            var response = await client.PostAsJsonAsync("api/v1/jobs", job);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

    }
}