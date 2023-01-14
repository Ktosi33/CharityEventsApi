using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using CharityEventsApi.Entities;
using Xunit;
using System.Threading.Tasks;
using CharityEventsApi.Models.DataTransferObjects;
using Newtonsoft.Json;
using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using CharityEventsApi.Tests.Integration.TestHealpers;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using System.Net.Http.Headers;

namespace CharityEventsApi.Tests.Integration.Controllers
{
    public class CharityEventControllerTests  : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;

        public CharityEventControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();

        }
        
        [Theory]
        [InlineData("New Title", "New Description", "Fund Target", 5555.55, 40, 1, true, true)]
        [InlineData("Lorem ipsum dolor sit amet.", "Fusce maximus sapien eget ipsum pulvinar egestas",
                                        "Sed elementum iaculis condimentum.", 2.1, 2, 1, true, false)]
        [InlineData("null", "false", "true", 2.1, 2, 1, false, true)]
        public async Task AddAllCharityEventDtoByForm_CreateAllCharityEvents_ReturnsOkResult
            (string title, string description, string fundTarget,
            decimal amountOfMoneyToCollect, int amountOfNeededVolunteers,
            int organizerId, bool isFundraising, bool isVolunteering)
        {
            //arrange
            var httpContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("test content"));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            
            httpContent.Add(new StringContent(title), "Title");
            httpContent.Add(new StringContent(description), "Description");
            httpContent.Add(new StringContent(fundTarget), "FundTarget");
            httpContent.Add(new StringContent(amountOfMoneyToCollect.ToString()), "AmountOfMoneyToCollect");
            httpContent.Add(new StringContent(amountOfNeededVolunteers.ToString()), "AmountOfNeededVolunteers");
            httpContent.Add(new StringContent(organizerId.ToString()), "idOrganizer");
            httpContent.Add(new StringContent(isFundraising.ToString()), "IsFundraising");
            httpContent.Add(new StringContent(isVolunteering.ToString()), "IsVolunteering");
            httpContent.Add(fileContent, "ImageCharityEvent", "newImage.jpeg");

            //act
            var response = await client.PostAsync("/v1/CharityEvent", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("New Title", "New Description", "Fund Target", 5555.55, 40, 0, true, true)]
        [InlineData("All null", "ccc", "aaa", 1.1, 3, 1, false, false)]
        [InlineData("", "", "", 0.0, 1, 1, false, false)] 
        public async Task AddAllCharityEventDtoByForm_whenCreateAllCharityEvents_ReturnsBadRequest
       (string title, string description, string fundTarget, decimal amountOfMoneyToCollect, 
            int amountOfNeededVolunteers, int organizerId, bool isFundraising, bool isVolunteering)
        {
            //arrange
            var httpContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("test content"));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            httpContent.Add(new StringContent(title), "Title");
            httpContent.Add(new StringContent(description), "Description");
            httpContent.Add(new StringContent(fundTarget), "FundTarget");
            httpContent.Add(new StringContent(amountOfMoneyToCollect.ToString()), "AmountOfMoneyToCollect");
            httpContent.Add(new StringContent(amountOfNeededVolunteers.ToString()), "AmountOfNeededVolunteers");
            httpContent.Add(new StringContent(organizerId.ToString()), "OrganizerId");
            httpContent.Add(new StringContent(isFundraising.ToString()), "IsFundraising");
            httpContent.Add(new StringContent(isVolunteering.ToString()), "IsVolunteering");
            httpContent.Add(fileContent, "ImageCharityEvent", "newImage.jpeg");

            //act
            var response = await client.PostAsync("/v1/CharityEvent", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task IsActive_DisactiveBySettingfalse_ReturnsOkResult()
        {       
            //act
            var response = await client.PatchAsync("/v1/CharityEvent/1?isActive=false", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
     
        [Fact]
        public async Task IdCharityEvent_GetCharityEvent_ReturnsOkResult()
        {
            //act
            var response = await client.GetAsync("/v1/CharityEvent/1");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("New title", "Example of description", 1)]
        [InlineData("Second new title ", null, 1)]
        public async Task EditCharityEventDto_EditCharityEvent_ReturnsOkResult
                                (string title, string description, int organizerId)
        {
            //arange
            EditCharityEventDto dto = new EditCharityEventDto
            {
                Title = title,
                Description = description,
                IdOrganizer = organizerId
            };
            var load = new StringContent(JsonConvert.SerializeObject(dto), UnicodeEncoding.UTF8, "application/json");

            //act
            var response = await client.PutAsync("/v1/CharityEvent/1", load);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("true", "true")]
        [InlineData("true", "false")]
        public async Task IsVerifiedIsActive_ChangeVerifyAndActive_ReturnsOkResult(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEvent/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("false", "true")]
        [InlineData("randomData", "randomData")]
        [InlineData("null", "null")]
        public async Task IsVerifiedIsActive_ChangeVerifyAndActive_ReturnsBadRequest(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEvent/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
