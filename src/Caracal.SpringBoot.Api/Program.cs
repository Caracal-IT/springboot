using Caracal.SpringBoot.Business;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpringBoot();

var app = builder.Build();

app.UseSpringBoot();

app.Run();