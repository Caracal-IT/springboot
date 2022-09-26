using System.Net;
using System.Text.Json;
using Caracal.SpringBoot.Admin.Api.Model;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace Caracal.SpringBoot.Admin.Api.Controllers;
[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
  private readonly IHttpClientFactory _httpClientFactory;

  public WeatherForecastController(IHttpClientFactory httpClientFactory, AsyncRetryPolicy _retryPolicy) {
    _httpClientFactory = httpClientFactory;
  }

  [HttpGet]
  public async Task<OkObjectResult> Get()
  {
    var client = _httpClientFactory.CreateClient("OpenWeather");
    var response = await client.GetFromJsonAsync<WeatherForecast>("/data/2.5/weather?lat=30&lon=-40&appid=5e1f46a94b593ec9dd1a9986073df06f");

    return Ok(response);
  }
}