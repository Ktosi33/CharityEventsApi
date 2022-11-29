using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using CharityEventsApi.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;


namespace CharityEventsApi.Tests.Integration.TestHealpers
{
    public class ClientInit
    {
        public HttpClient Client { get; }
        public ClientInit()
        {
            var factory = new WebApplicationFactory<Program>();
            Client = factory.WithWebHostBuilder(builder =>
            {

                builder.ConfigureServices(services =>
                {
                    var dbContext = services.FirstOrDefault(dbContext => dbContext.ServiceType == typeof(DbContextOptions<CharityEventsDbContext>));
                    if(dbContext is not null)
                    { 
                    services.Remove(dbContext);
                    }

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
        
    }
}
