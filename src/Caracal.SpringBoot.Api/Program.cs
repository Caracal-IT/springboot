using Caracal.SpringBoot.Api.Deposits;
using Caracal.SpringBoot.Api.Withdrawals;
using Caracal.SpringBoot.Application;
using Caracal.SpringBoot.Data;
using Caracal.SpringBoot.Data.Postgres;
using Caracal.SpringBoot.Data.Postgres.Models.Deposits;
using Caracal.SpringBoot.Kafka;
using Caracal.Web.Core.DI;
using Caracal.Web.Core.Messaging;
using Elastic.Apm.NetCoreAll;
using Elastic.Apm.StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args)
                            .WithSerilog();

builder.Services.AddScoped(typeof(GetWithdrawals));
builder.Services.AddScoped(typeof(AddDeposit));

builder.Services
       .AddSpringBoot()
       .AddSpringBootData()
       .AddSingleton<IWriteOnlyQueue, Producer>();

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("StringBoot")));

builder.Services.AddSingleton<IConnectionMultiplexer>(_ => {
  var connStr = builder.Configuration.GetConnectionString("Redis");
  var c = ConnectionMultiplexer.Connect(connStr!);
  c.UseElasticApm();
  
  return c;
});

var app = builder.Build();
app.UseAllElasticApm(app.Configuration);

app.UseSpringBoot();

app.MapGet("withdrawals", async (GetWithdrawals getWithdrawals, CancellationToken cancellationToken) => 
  await getWithdrawals.ExecuteAsync(cancellationToken));

app.MapPost("deposits", async (AddDeposit addDeposit, [FromBody] Deposit deposit, CancellationToken cancellationToken) => 
  await addDeposit.ExecuteAsync(deposit, cancellationToken));

app.Run();