using Caracal.SpringBoot.Api.Withdrawals;
using Caracal.SpringBoot.Application;
using Caracal.SpringBoot.Data;
using Caracal.SpringBoot.Data.Postgres;
using Caracal.Web.Core.DI;
using Elastic.Apm.NetCoreAll;
using Elastic.Apm.StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args)
                            .WithSerilog();

builder.Services.AddScoped(typeof(GetWithdrawals));

builder.Services
       .AddSpringBoot()
       .AddSpringBootData();

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("StringBoot")));

builder.Services.AddSingleton<IConnectionMultiplexer>(_ => {
  var connStr = builder.Configuration.GetConnectionString("Redis");
  var c = ConnectionMultiplexer.Connect(connStr!);
  c.UseElasticApm();
  
  return c;
});

var app = builder.Build();

app.UseSpringBoot();

app.MapGet("withdrawals", async (GetWithdrawals getWithdrawals) => await getWithdrawals.ExecuteAsync());

app.UseAllElasticApm(app.Configuration);

app.Run();