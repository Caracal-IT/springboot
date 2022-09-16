using Caracal.SpringBoot.Data.Postgres;
using Caracal.SpringBoot.Kafka;
using Caracal.SpringBoot.Workers.General.Workers;
using Caracal.Web.Core.DI;
using Caracal.Web.Core.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateDefaultBuilder(args)
                  .WithSerilog();

var host = builder.ConfigureServices(RegisterDb)
                  .ConfigureServices(RegisterServices)
                  .ConfigureServices(RegisterWorkers)
                  .Build();

host.Run();

void RegisterDb(HostBuilderContext context, IServiceCollection services) {
  services.AddDbContext<DataContext>(options => 
    options.UseNpgsql(context.Configuration.GetConnectionString("StringBoot"))
  );
}

void RegisterServices(IServiceCollection services) {
  services.AddSingleton<IReadonlyQueue, Consumer>();
}

void RegisterWorkers(IServiceCollection services) {
  services.AddHostedService<Worker>()
          .AddHostedService<Worker2>();
}