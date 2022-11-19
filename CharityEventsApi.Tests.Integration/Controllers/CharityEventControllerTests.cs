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

namespace CharityEventsApi.Tests.Integration.Controllers
{
    public class CharityEventControllerTests 
    {
        private HttpClient client;
        public CharityEventControllerTests()
        {
            var factory = new WebApplicationFactory<Program>();
            client = factory.WithWebHostBuilder(builder =>
            {
                
                builder.ConfigureServices(services =>
                {
                    var dbContext = services.FirstOrDefault(dbContext => dbContext.ServiceType == typeof(DbContextOptions<CharityEventsDbContext>));
                    services.Remove(dbContext);

                    string _dbName = Guid.NewGuid().ToString();
                    services.AddDbContext<CharityEventsDbContext>( options => options.UseInMemoryDatabase(databaseName: _dbName)
                   .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
                    services.AddTransient<SeedTestData>();
                    
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var seedTestData = scope.ServiceProvider.GetRequiredService<SeedTestData>();
                        seedTestData.Seed();
                    }

                }); 
                
            })
           .CreateClient();
          
        }
        
        [Theory]
        [InlineData("Aaa", "bbb", "ccc", 2.1, 2, 1, true, true)]
        [InlineData("TF", "bbb", "ccc", 2.1, 2, 1, true, false)]
        [InlineData("FT", "bbb", "ccc", 2.1, 2, 1, false, true)]
        [InlineData("All null", "ccc", "aaa", null, null, 1, false, false)]
        public async Task AddCharityEvents_WithAllCharityEventsDto_ReturnsOkResult
            (string title, string description, string fundTarget,
            decimal amountOfMoneyToCollect, int amountOfNeededVolunteers,
            int organizerId, bool isFundraising, bool isVolunteering)
        {
            //arrange
            var dto = new AddAllCharityEventsDto() {
                Title = title, 
                Description = description, 
                FundTarget = fundTarget, 
                AmountOfMoneyToCollect = amountOfMoneyToCollect,
                AmountOfNeededVolunteers = amountOfNeededVolunteers,
                OrganizerId = organizerId,
                isFundraising = isFundraising,
                isVolunteering = isVolunteering
            };
            var load = new StringContent(JsonConvert.SerializeObject(dto), UnicodeEncoding.UTF8, "application/json");
            
            //act

            var response = await client.PostAsync("/v1/CharityEvent", load);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(null, null, null, null, null, 1, false, false)]
       //[InlineData("", null, null, null, null, 1, false, false)] //that test wouldnt pass when validators are added
        public async Task AddCharityEvents_WithAllCharityEventsDto_ReturnsBadRequest
       (string title, string description, string fundTarget, decimal amountOfMoneyToCollect, int amountOfNeededVolunteers, int organizerId, bool isFundraising, bool isVolunteering)
        {
            //arrange
            var dto = new AddAllCharityEventsDto()
            {
                Title = title,
                Description = description,
                FundTarget = fundTarget,
                AmountOfMoneyToCollect = amountOfMoneyToCollect,
                AmountOfNeededVolunteers = amountOfNeededVolunteers,
                OrganizerId = organizerId,
                isFundraising = isFundraising,
                isVolunteering = isVolunteering
            };
            var load = new StringContent(JsonConvert.SerializeObject(dto), UnicodeEncoding.UTF8, "application/json");

            //act

            var response = await client.PostAsync("/v1/CharityEvent", load);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DisactiveCharityEvents_CreateNewAndDisactiveHim_ReturnsOkResult()
        {       
            //act
            var response = await client.PatchAsync("/v1/CharityEvent/1?isActive=false", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
     
        [Fact]
        public async Task Get_ById_ReturnsOkResult()
        {
            //act
            var response = await client.GetAsync("/v1/CharityEvent/1");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("NewTitle", "Desc", 1,1)]
        [InlineData("NewTitlea", null, 1,1)]
        public async Task EditCharityEvents_FromEditCharityEventDto_ReturnsOkResult(string title, string description, int organizerId, int imageId)
        {
            //arange
            EditCharityEventDto dto = new EditCharityEventDto
            {
                Title = title,
                Description = description,
                OrganizerId = organizerId,
                ImageId = imageId
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
        public async Task setActiveVerifyCharityEvents_VerifyAndActive_ReturnsOkResult(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEvent/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("false", "true")]
        [InlineData("asd", "asd")]
        [InlineData("null", "null")]
        public async Task setActiveVerifyCharityEvents_VerifyAndActive_ReturnsBadRequestResult(string isVerified, string isActive)
        {
            //act
            var response = await client.PatchAsync($"/v1/CharityEvent/1?isVerified={isVerified}&isActive={isActive}", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
