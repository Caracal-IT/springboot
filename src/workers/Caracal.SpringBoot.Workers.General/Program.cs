using Caracal.SpringBoot.Workers.General.Workers;

var host = Host.CreateDefaultBuilder(args)
  .ConfigureServices(RegisterWorkers)
  .Build();

host.Run();

void RegisterWorkers(IServiceCollection services) =>
  services.AddHostedService<Worker>()
          .AddHostedService<Worker2>();