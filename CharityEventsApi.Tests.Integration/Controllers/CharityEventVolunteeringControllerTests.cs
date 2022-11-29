﻿using CharityEventsApi.Entities;
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
    public class CharityEventVolunteeringControllerTests
    {
        private readonly HttpClient client;
        public CharityEventVolunteeringControllerTests()
        {
            client = new ClientInit().Client;
        }

        [Fact]
        public async Task givenIsActive_whenDisactiveVolunteering_thenReturnsOkResult()
        {
            //act
            var response = await client.PatchAsync("/v1/CharityEventVolunteering/1?isActive=false", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task givenIdVolunteering_whenGetVolunteering_thenReturnsOkResult()
        {
            //act
            var response = await client.GetAsync("/v1/CharityEventVolunteering/1");

            //assert
             response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task givenEditCharityEventVolunteeringDto_whenEditVolunteering_thenReturnsOkResult(int amountOfNeededVolunteers)
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
        public async Task givenIsVerifiedIsActiveVolunteering_whenChangeVerifyAndActive_thenReturnsOkResult(string isVerified, string isActive)
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
        public async Task givenIsVerifiedIsActiveVolunteering_whenChangeVerifyAndActive_thenReturnsBadRequest(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEventVolunteering/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
