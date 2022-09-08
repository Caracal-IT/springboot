using Caracal.SpringBoot.Business;
using Caracal.SpringBoot.Data;
using Elastic.Apm.NetCoreAll;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddSpringBoot()
       .AddSpringBootData();

var app = builder.Build();

app.UseAllElasticApm(app.Configuration);
app.UseSpringBoot();

app.Run();