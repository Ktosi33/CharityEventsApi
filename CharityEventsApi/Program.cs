using CharityEventsApi;
using CharityEventsApi.Entities;
using CharityEventsApi.Middleware;
using CharityEventsApi.Services.Account;
using CharityEventsApi.Services.CharityEvent;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}
).AddJwtBearer(
    cfg =>
    {     
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
        };
    }
    );



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CharityEventsApi",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



//Configure DBContext
builder.Services.AddDbContext<CharityEventsDbContext>(
    option => option.UseMySql(
        builder.Configuration.GetConnectionString("CharityEventsConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("CharityEventsConnectionString"))
        )
    );
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ICharityEventService, CharityEventService>();
builder.Services.AddScoped<ICharityEventVolunteeringService, CharityEventVolunteeringService>();
builder.Services.AddScoped<ICharityEventFundraisingService, CharityEventFundraisingService>();
builder.Services.AddTransient<ICharityEventFactory, CharityEventFactory>();
builder.Services.AddTransient<ICharityEventFundraisingFactory, CharityEventFundraisingFactory>();
builder.Services.AddTransient<ICharityEventVolunteeringFactory, CharityEventVolunteeringFactory>();
builder.Services.AddTransient<ICharityEventFactoryFacade, CharityEventFactoryFacade>();

var app = builder.Build();



app.UseCors(x => x
             .AllowAnyMethod()
             .AllowAnyHeader()
             .SetIsOriginAllowed(origin => true) // allow any origin
             .AllowCredentials()); // allow credentials

//app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
