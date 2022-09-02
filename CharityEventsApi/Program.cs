using CharityEventsApi.Entities;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Configure DBContext
/* builder.Services.AddDbContext<CharityEventsDbContext>(
    option => option.UseMySql(
        builder.Configuration.GetConnectionString("CharityEventsConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("CharityEventsConnectionString"))
        )
    );
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
