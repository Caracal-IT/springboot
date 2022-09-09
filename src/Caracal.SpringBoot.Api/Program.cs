using Caracal.SpringBoot.Application;
using Caracal.SpringBoot.Data;
using Caracal.Web.Core.DI;
using Elastic.Apm.NetCoreAll;

var builder = WebApplication.CreateBuilder(args)
                            .WithSerilog();

builder.Services
       .AddSpringBoot()
       .AddSpringBootData();

var app = builder.Build();

app.UseAllElasticApm(app.Configuration);
app.UseSpringBoot();

app.Run();