using Caracal.SpringBoot.Api.WeatherForecasts.Model;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Caracal.SpringBoot.Api.WeatherForecasts; 

public sealed class GetCurrentForecast {
  private readonly IHttpClientFactory _httpClientFactory;
  
  public GetCurrentForecast(IHttpClientFactory httpClientFactory) {
    _httpClientFactory = httpClientFactory;
  }

  public async Task<IResult> ExecuteAsync(CancellationToken cancellationToken) {
    var client = _httpClientFactory.CreateClient();
    var response = await client.GetFromJsonAsync<WeatherForecast>("http://localhost:9090/weatherForecast", cancellationToken);

    return Results.Ok(response);
  }
}