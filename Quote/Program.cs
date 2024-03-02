using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quote.Helper;
using Quote.Interfaces.ServiceInterface;

using Quote.Repositorys;
using Quote.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Quote.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserInterface,UserService>();
builder.Services.AddDbContext<DB_SWDContext>();
builder.Services.AddAutoMapper(typeof(AutoMapperHandler).Assembly);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddAuthentication().AddGoogle(option =>
{
    option.ClientId = builder.Configuration.GetSection("Google").GetValue<string>("ClientId");
    option.ClientSecret = builder.Configuration.GetSection("Google").GetValue<string>("ClientSecret");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
