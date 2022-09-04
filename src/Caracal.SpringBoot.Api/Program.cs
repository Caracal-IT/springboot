using Caracal.SpringBoot.Business;
using Caracal.SpringBoot.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddSpringBoot()
       .AddSpringBootData();

var app = builder.Build();

app.UseSpringBoot();

app.Run();