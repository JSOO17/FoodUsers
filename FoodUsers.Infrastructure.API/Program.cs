using FoodUsers.Application.Services;
using FoodUsers.Application.Services.Interfaces;
using FoodUsers.Domain.Intefaces.API;
using FoodUsers.Domain.Intefaces.SPI;
using FoodUsers.Domain.UserCases;
using FoodUsers.Infrastructure.API.Security;
using FoodUsers.Infrastructure.Data.Adapters;
using FoodUsers.Infrastructure.Data.Models;
using FoodUsers.Infrastructure.Security.Encrypt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUserServicesPort, UserUserCases>();
builder.Services.AddTransient<IUserPersistencePort, UserAdapter>();
builder.Services.AddTransient<IUserServices, UserServices>();

builder.Services.AddTransient<IUserEncryptPort, UserEncrypt>();

builder.Services.AddDbContext<foodusersContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("foodusers"), ServerVersion.Parse("10.4.28-mariadb")));

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddLogging();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var JwtSettings = builder.Configuration.GetSection("Jwt").Get<JWTSettings>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtSettings.Issuer,
            ValidAudience = JwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key)),
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
