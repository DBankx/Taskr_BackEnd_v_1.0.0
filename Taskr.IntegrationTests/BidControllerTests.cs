﻿using System;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Newtonsoft.Json;
using Taskr.Commands.Bid;
using Taskr.Commands.Task;
using Taskr.Domain;
using Xunit;

namespace Taskr.IntegrationTests
{
    public class BidControllerTests : ControllerTestBase
    {
        public BidControllerTests(WebApiTestFactory factory) : base(factory)
        {
            
        }

        [Fact]
        public async Task Get_All_Job_Bids_Should_Return_Not_Found_With_Wrong_JobId()
        {
            await CreateUserAndAuthorizeAsync();
            
            var response = await client.GetAsync($"api/v1/bids/get-bid/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_Bid_Price_Lower_Than_Job_Price_Should_Return_BadRequest()
        {
            // Arrange
            await CreateUserAndAuthorizeAsync();
            
            var jobGuid = Guid.NewGuid();
            
            // create job
            var job = new CreateJobCommand
            {
                Id = jobGuid,
                Title = "test job",
                Description = "this is a test job",
                InitialPrice = 40.32m
            };

            await  client.PostAsJsonAsync("api/v1/jobs", job);
            
            // Act
            var bid = new CreateBidCommand
            {
                Description = "test bid",
                Price = 50.00m
            };

            var response = await client.PostAsJsonAsync($"api/v1/bids/{jobGuid}", bid);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetBidById_Should_Return_Bid_If_Bid_Exists()
        {
            // Assert
            await CreateUserAndAuthorizeAsync();
            
            var jobGuid = Guid.NewGuid();
            
            // create job
            var job = new CreateJobCommand
            {
                Id = jobGuid,
                Title = "test job",
                Description = "this is a test job",
                InitialPrice = 40.32m
            };

            await client.PostAsJsonAsync("api/v1/jobs", job);
            
            var bidId = Guid.NewGuid();
            
            var bid = new CreateBidCommand
            {
                Id = bidId,
                Description = "test bid",
                Price = 30.00m,
                JobId = jobGuid
            };

            await client.PostAsJsonAsync($"api/v1/bids/{jobGuid}", bid);
            
            // Act
            var response = await client.GetAsync($"api/v1/bids/get-bid/{bidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiJobResponse = JsonConvert.DeserializeObject<Bid>(await response.Content.ReadAsStringAsync());
            apiJobResponse.Id.Should().Be(bidId);
        }
    }
}