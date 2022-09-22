using Caracal.SpringBoot.Admin.Api.Model;

namespace Caracal.SpringBoot.Admin.Api.Controllers; 
//5e1f46a94b593ec9dd1a9986073df06f
[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
  private static readonly string[] Summaries = 
  {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  private readonly ILogger<WeatherForecastController> _logger;
  private readonly IHttpClientFactory _httpClientFactory;

  public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory) {
    _logger = logger;
    _httpClientFactory = httpClientFactory;
  }

  [HttpGet]
  public async Task<OkObjectResult> Get()
  {
    _logger.LogDebug("Calling Weather Data");
    var client = _httpClientFactory.CreateClient();
    var response = await client.GetFromJsonAsync<WeatherForecast>("https://api.openweathermap.org/data/2.5/weather?lat=30&lon=-40&appid=5e1f46a94b593ec9dd1a9986073df06f");

    return Ok(response);
  }
}