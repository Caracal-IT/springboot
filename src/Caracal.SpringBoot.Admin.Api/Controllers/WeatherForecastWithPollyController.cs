using System.Net;
using System.Text.Json;
using Caracal.SpringBoot.Admin.Api.Model;
using Polly;
using Polly.Retry;

namespace Caracal.SpringBoot.Admin.Api.Controllers; 
//5e1f46a94b593ec9dd1a9986073df06f
[ApiController]
[Route("[controller]")]
[Obsolete]
public sealed class WeatherForecastWithPollyController : ControllerBase
{
  private readonly IHttpClientFactory _httpClientFactory;

  private readonly AsyncRetryPolicy _retryPolicy;

  public WeatherForecastWithPollyController(IHttpClientFactory httpClientFactory) {
    _httpClientFactory = httpClientFactory;

    _retryPolicy = Policy.Handle<HttpRequestException>()
                         .RetryAsync(3); 

    //_retryPolicy = Policy.Handle<HttpRequestException>(exception => exception.Message != "This is a fake request exception")
    //                     .WaitAndRetryAsync(3, times => TimeSpan.FromMilliseconds(times * 100));
  }

  [HttpGet]
  public async Task<OkObjectResult> Get()
  {
    var client = _httpClientFactory.CreateClient();
    
    var response = await _retryPolicy.ExecuteAsync(async () => {
      if (Random.Shared.Next(1, 3) == 1)
        throw new HttpRequestException("This is a fake request exception");
      
      var result = await client.GetAsync(
        "https://api.openweathermap.org/data/2.5/weather?lat=30&lon=-40&appid=5e1f46a94b593ec9dd1a9986073df06f"
      );
      
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