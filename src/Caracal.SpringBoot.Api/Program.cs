using Caracal.Web.MediatR.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(x => x.AsScoped(), typeof(Caracal.SpringBoot.Business.Main));
builder.Services.AddScoped<Caracal.SpringBoot.Business.Services.GuidService>();

var app = builder.Build();

app.RegisterHandlers(typeof(Caracal.SpringBoot.Business.Main));

app.Run();