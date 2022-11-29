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
    public class CharityEventFundraisingControllerTests
    {
        private readonly HttpClient client;
        public CharityEventFundraisingControllerTests()
        {
            client = new ClientInit().Client;
        }

        [Fact]
        public async Task givenIsActive_whenDisactiveFundraising_thenReturnsOkResult()
        {
            //act
            var response = await client.PatchAsync("/v1/CharityEventFundraising/1?isActive=false", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task givenIdFundraising_whenGetFundraising_thenReturnsOkResult()
        {
            //act
            var response = await client.GetAsync("/v1/CharityEventFundraising/1");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("NewTitle", 1.1)]
        [InlineData("Test", 2)]
        public async Task givenEditCharityEventFundraisingDto_whenEditFundraising_thenReturnsOkResult(string fundTarget, decimal amountOfMoneyToCollect)
        {
            //arange
            EditCharityEventFundraisingDto dto = new EditCharityEventFundraisingDto
            {
               FundTarget = fundTarget,
               AmountOfMoneyToCollect = amountOfMoneyToCollect
            };
            var load = new StringContent(JsonConvert.SerializeObject(dto), UnicodeEncoding.UTF8, "application/json");

            //act
            var response = await client.PutAsync("/v1/CharityEventFundraising/1", load);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("true", "true")]
        [InlineData("true", "false")]
        public async Task givenIsVerifiedIsActiveFundraising_whenChangeVerifyAndActive_thenReturnsOkResult(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEvent/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("false", "true")]
        [InlineData("isVerified", "isActive")]
        [InlineData("null", "null")]
        public async Task givenIsVerifiedIsActiveFundraising_whenChangeVerifyAndActive_thenReturnsBadRequest(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEventFundraising/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }


    }
}
