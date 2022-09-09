using Caracal.SpringBoot.Workers.General.Workers;
using Caracal.Web.Core.DI;

var builder = Host.CreateDefaultBuilder(args)
                  .WithSerilog();

var host = builder.ConfigureServices(RegisterWorkers)
                  .Build();

host.Run();

void RegisterWorkers(IServiceCollection services) {
  services.AddHostedService<Worker>()
          .AddHostedService<Worker2>();
}