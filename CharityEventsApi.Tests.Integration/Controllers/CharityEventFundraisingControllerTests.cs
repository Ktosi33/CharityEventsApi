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
      
        private HttpClient client;
        public CharityEventFundraisingControllerTests()
        {
            var factory = new WebApplicationFactory<Program>();
            client = factory.WithWebHostBuilder(builder =>
            {

                builder.ConfigureServices(services =>
                {
                    var dbContext = services.FirstOrDefault(dbContext => dbContext.ServiceType == typeof(DbContextOptions<CharityEventsDbContext>));
                    services.Remove(dbContext);

                    string _dbName = Guid.NewGuid().ToString();
                    services.AddDbContext<CharityEventsDbContext>(options => options.UseInMemoryDatabase(databaseName: _dbName)
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

        [Fact]
        public async Task EndFundraising_CreateNewAndEndHim_ReturnsOkResult()
        {
            //act
            var response = await client.PutAsync("/v1/CharityEventFundraising/end/1", null);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetFundraising_ById_ReturnsOkResult()
        {
            //act
            var response = await client.GetAsync("/v1/CharityEventFundraising/1");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("NewTitle", 1.1)]
        [InlineData("Test", 2)]
        public async Task EditFundraising_FromEditCharityEventDto_ReturnsOkResult(string fundTarget, decimal amountOfMoneyToCollect)
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
       

    }
}
