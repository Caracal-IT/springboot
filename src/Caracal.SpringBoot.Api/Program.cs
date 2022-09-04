using Caracal.Web.MediatR.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(x => x.AsScoped(), typeof(Caracal.SpringBoot.Business.Main));
builder.Services.AddScopedServices(typeof(Caracal.SpringBoot.Business.Main));

var app = builder.Build();

app.RegisterHandlers(typeof(Caracal.SpringBoot.Business.Main));

app.Run();