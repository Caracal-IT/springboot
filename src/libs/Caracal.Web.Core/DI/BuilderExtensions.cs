using System.Reflection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Caracal.Web.Core.DI; 

public static class BuilderExtensions {
  public static WebApplicationBuilder WithSerilog(this WebApplicationBuilder builder) {
    WithSerilog(builder.Host);
    
    return builder;
  }
    
  public static IHostBuilder WithSerilog(this IHostBuilder builder) {
    ConfigureLogging();
    builder.UseSerilog();
    
    return builder;
    
    void ConfigureLogging() {
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "development";
      var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
        .Build();

      Log.Logger = new LoggerConfiguration()
         .Enrich.FromLogContext()
         .Enrich.WithExceptionDetails()
         .WriteTo.Debug()
         .WriteTo.Console()
         .WriteTo.Elasticsearch(ConfigureElasticSink())
         .Enrich.WithProperty("Environment", environment)
         .ReadFrom.Configuration(configuration)
         .CreateLogger();

      ElasticsearchSinkOptions ConfigureElasticSink() =>
        new(new Uri(configuration["ElasticConfiguration:Uri"] ?? string.Empty)) {
          AutoRegisterTemplate = true,
          IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        };
    }
  }
}