using System.Reflection;
using Serilog;
using Serilog.Sinks.Elasticsearch;

using Caracal.SpringBoot.Business;
using Caracal.SpringBoot.Data;
using Elastic.Apm.NetCoreAll;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging();
builder.Host.UseSerilog();

builder.Services
       .AddSpringBoot()
       .AddSpringBootData();

var app = builder.Build();

app.UseAllElasticApm(app.Configuration);
app.UseSpringBoot();

app.Run();

void ConfigureLogging()
{
       var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
       var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile(
                     $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                     optional: true)
              .Build();

       Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .Enrich.WithExceptionDetails()
              .WriteTo.Debug()
              .WriteTo.Console()
              .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment??"development"))
              .Enrich.WithProperty("Environment", environment??"development")
              .ReadFrom.Configuration(configuration)
              .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
{
       return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"] ?? string.Empty))
       {
              AutoRegisterTemplate = true,
              IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
       };
}