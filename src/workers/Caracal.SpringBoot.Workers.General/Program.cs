using Caracal.SpringBoot.Workers.General.Workers;

using System.Reflection;
using Serilog;
using Serilog.Sinks.Elasticsearch;

using Serilog.Exceptions;

var builder = Host.CreateDefaultBuilder(args);

ConfigureLogging();

builder.UseSerilog();

var host =   builder.ConfigureServices(RegisterWorkers)
                    .Build();

host.Run();

void RegisterWorkers(IServiceCollection services) =>
  services.AddHostedService<Worker>()
          .AddHostedService<Worker2>();
          
          
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