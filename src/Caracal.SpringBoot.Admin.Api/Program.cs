using System.Net;
using Caracal.SpringBoot.Templates.Services;
using Caracal.Web.Core.DI;
using Elastic.Apm.NetCoreAll;
using Polly;
using Polly.Contrib.WaitAndRetry;

var builder = WebApplication.CreateBuilder(args)
                            .WithSerilog();

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//   .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddHttpClient();
builder.Services
  .AddHttpClient("OpenWeather", client => {
    client.BaseAddress = new Uri("https://api.openweathermap.org");
    client.DefaultRequestHeaders.Add("User-Agent", "springboot-admin");
  })
  //.AddPolicyHandler(Policy<HttpResponseMessage>
  //  .Handle<HttpRequestException>()
  //  .OrResult(x => x.StatusCode is >= HttpStatusCode.InternalServerError or HttpStatusCode.RequestTimeout)
  //  .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5))
  //);
  .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITemplateService>(p => new TemplateService(p.GetService<IHostEnvironment>()!.ContentRootFileProvider));

var app = builder.Build();

app.UseAllElasticApm(app.Configuration);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();