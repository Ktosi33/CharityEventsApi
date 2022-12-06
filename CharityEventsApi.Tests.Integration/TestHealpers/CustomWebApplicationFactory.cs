using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CharityEventsApi.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Authorization.Policy;

namespace CharityEventsApi.Tests.Integration.TestHealpers
{
    public class CustomWebApplicationFactory<TProgram>: WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            
                builder.ConfigureServices(services =>
                {
                    var dbContext = services.FirstOrDefault(dbContext => dbContext.ServiceType == typeof(DbContextOptions<CharityEventsDbContext>));
                    if (dbContext is not null)
                    {
                        services.Remove(dbContext);
                    }

                    string _dbName = Guid.NewGuid().ToString();
                    services.AddDbContext<CharityEventsDbContext>(options => options.UseInMemoryDatabase(databaseName: _dbName)
                  .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    services.AddTransient<SeedTestData>();

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var seedTestData = scope.ServiceProvider.GetRequiredService<SeedTestData>();
                        seedTestData.Seed();
                    }

                });

         
        }
    }
}
