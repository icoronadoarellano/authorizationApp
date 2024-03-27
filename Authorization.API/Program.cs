using Microsoft.Extensions.Configuration;
using Authorization.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Authorization.DataAccess;
using Arch.EntityFrameworkCore.UnitOfWork;
using Authorization.BusinessLogic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Authorization.API;

var builder = WebApplication.CreateBuilder(args);
var appsettings = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null  ? $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json" : "appsettings.json";

    var config = new ConfigurationBuilder()
        .AddJsonFile(appsettings, optional: false)
        .Build();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = config.GetConnectionString("AuthorizationDatabase");
Console.WriteLine(connectionString);
builder.Services.AddDbContext<PermissionContext>(opt => opt.UseSqlServer(config.GetConnectionString("AuthorizationDatabase")))
    .AddUnitOfWork<PermissionContext>();

builder.Services.AddTransient<PermissionInitializer>();
builder.Services.AddTransient<IPermissionBL, PermissionBL>();
builder.Services.AddTransient<IPermissionDA, PermissionDA>();


builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
}));

builder.Services.AddElasticSearch(builder.Configuration);

var app = builder.Build();
var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var initialiser = services.GetRequiredService<PermissionInitializer>();

initialiser.Run();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();