using CharityEventsApi.Entities;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Tests.Integration.TestHealpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CharityEventsApi.Tests.Integration.Controllers
{
    public class CharityEventVolunteeringControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        public CharityEventVolunteeringControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task IsActive_DisactiveVolunteering_ReturnsOkResult()
        {
            //act
            var response = await client.PatchAsync("/v1/CharityEventVolunteering/1?isActive=false", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task IdVolunteering_GetVolunteering_ReturnsOkResult()
        {
            //act
            var response = await client.GetAsync("/v1/CharityEventVolunteering/1");

            //assert
             response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public async Task EditCharityEventVolunteeringDto_EditVolunteering_ReturnsOkResult(int amountOfNeededVolunteers)
        {
            //arange
            EditCharityEventVolunteeringDto dto = new()
            {
               AmountOfNeededVolunteers = amountOfNeededVolunteers
            };
            var load = new StringContent(JsonConvert.SerializeObject(dto), UnicodeEncoding.UTF8, "application/json");

            //act
            var response = await client.PutAsync("/v1/CharityEventVolunteering/1", load);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("true", "true")]
        [InlineData("true", "false")]
        public async Task IsVerifiedIsActiveVolunteering_ChangeVerifyAndActive_ReturnsOkResult(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEventVolunteering/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("false", "true")]
        [InlineData("1", "1")]
        [InlineData("null", "null")]
        public async Task IsVerifiedIsActiveVolunteering_ChangeVerifyAndActive_ReturnsBadRequest(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEventVolunteering/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
