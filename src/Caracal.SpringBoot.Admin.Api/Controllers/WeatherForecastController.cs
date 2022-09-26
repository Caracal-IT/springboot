using System.Net;
using System.Text.Json;
using Caracal.SpringBoot.Admin.Api.Model;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace Caracal.SpringBoot.Admin.Api.Controllers; 
//5e1f46a94b593ec9dd1a9986073df06f
[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
  private readonly ILogger<WeatherForecastController> _logger;
  private readonly IHttpClientFactory _httpClientFactory;

  private readonly AsyncRetryPolicy _retryPolicy;

  //private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy2 =
  //  Policy<HttpResponseMessage>.Handle<HttpRequestException>()
  //    .OrResult(x => x.StatusCode is >= HttpStatusCode.InternalServerError or HttpStatusCode.RequestTimeout)
  //    .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));

  public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory) {
    _logger = logger;
    _httpClientFactory = httpClientFactory;

    //_retryPolicy = Policy.Handle<HttpRequestException>()
    //                     .RetryAsync(3); 

    _retryPolicy = Policy.Handle<HttpRequestException>(exception => exception.Message != "This is a fake request exception")
                         .WaitAndRetryAsync(3, times => TimeSpan.FromMilliseconds(times * 100));
  }

  [HttpGet]
  public async Task<OkObjectResult> Get()
  {
    _logger.LogDebug("Calling Weather Data");
    //var client = _httpClientFactory.CreateClient();
    var client = _httpClientFactory.CreateClient("OpenWeather");

    var response = await _retryPolicy.ExecuteAsync(async () => {
      //if (Random.Shared.Next(1, 3) == 1)
        //throw new HttpRequestException("This is a fake request exception");
      
      //var result = await client.GetAsync(
      //  "https://api.openweathermap.org/data/2.5/weather?lat=30&lon=-40&appid=5e1f46a94b593ec9dd1a9986073df06f"
      //);
      
      var result = await client.GetAsync("/data/2.5/weather?lat=30&lon=-40&appid=5e1f46a94b593ec9dd1a9986073df06f");

      if (result.StatusCode == HttpStatusCode.NotFound)
        return null;

      var resultString = await result.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<WeatherForecast>(resultString);
    });
    
    //var response = await client.GetFromJsonAsync<WeatherForecast>(
    //  "https://api.openweathermap.org/data/2.5/weather?lat=30&lon=-40&appid=5e1f46a94b593ec9dd1a9986073df06f"
    //);

    return Ok(response);
  }
}