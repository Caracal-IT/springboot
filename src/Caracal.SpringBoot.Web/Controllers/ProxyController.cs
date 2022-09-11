using Microsoft.AspNetCore.Mvc;

namespace Caracal.SpringBoot.Web.Controllers; 

[ApiController]
[Route("[controller]")]
public class ProxyController : ControllerBase
{
  private Uri _baseUri = new Uri("http://localhost:8200");
  private readonly HttpClient _httpClient;
  
  public ProxyController(HttpClient httpClient) {
    _httpClient = httpClient;
  }

  // /intake/v2/rum/events 
  [HttpGet("/intake/v2/rum/events")]
  public async Task<HttpResponseMessage> Get()
  {
    string absoluteUrl = _baseUri.ToString() + "/intake/v2/rum/events/" + Request.Query;
    var proxyRequest = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
    foreach (var header in Request.Headers)
    {
      proxyRequest.Headers.Add(header.Key, header.Value.ToString());
    }

    return await _httpClient.SendAsync(proxyRequest, HttpCompletionOption.ResponseContentRead);
  }

  [HttpPost("/intake/v2/rum/events")]
  public async Task<HttpResponseMessage> Post()
  {
    string absoluteUrl = _baseUri.ToString() + "/" + Request.Query;
    var proxyRequest = new HttpRequestMessage(HttpMethod.Post, absoluteUrl);
    foreach (var header in Request.Headers)
    {
      proxyRequest.Headers.Add(header.Key, header.Value.ToString());
    }
   
    var streamContent = new StreamContent(Request.Body);
    proxyRequest.Content = streamContent;

    return await _httpClient.SendAsync(proxyRequest, HttpCompletionOption.ResponseContentRead);
  }
  
  [HttpPatch("/intake/v2/rum/events")]
  public async Task<HttpResponseMessage> Fetch()
  {
    string absoluteUrl = _baseUri.ToString() + "/" + Request.Query;
    var proxyRequest = new HttpRequestMessage(HttpMethod.Patch, absoluteUrl);
    foreach (var header in Request.Headers)
    {
      proxyRequest.Headers.Add(header.Key, header.Value.ToString());
    }
    
    var streamContent = new StreamContent(Request.Body);
    proxyRequest.Content = streamContent;

    return await _httpClient.SendAsync(proxyRequest, HttpCompletionOption.ResponseContentRead);
  }

  [HttpPut("/intake/v2/rum/events")]
  public async Task<HttpResponseMessage> Put()
  {
    string absoluteUrl = _baseUri.ToString() + "/" + Request.Query;
    var proxyRequest = new HttpRequestMessage(HttpMethod.Put, absoluteUrl);
    foreach (var header in Request.Headers)
    {
      proxyRequest.Headers.Add(header.Key, header.Value.ToString());
    }
    
    var streamContent = new StreamContent(Request.Body);
    proxyRequest.Content = streamContent;

    return await _httpClient.SendAsync(proxyRequest, HttpCompletionOption.ResponseContentRead);
  }

  [HttpDelete("/intake/v2/rum/events")]
  public async Task<HttpResponseMessage> Delete()
  {
    string absoluteUrl = _baseUri.ToString() + "/" + Request.Query;
    var proxyRequest = new HttpRequestMessage(HttpMethod.Delete, absoluteUrl);
    foreach (var header in Request.Headers)
    {
      proxyRequest.Headers.Add(header.Key, header.Value.ToString());
    }

    var streamContent = new StreamContent(Request.Body);
    proxyRequest.Content = streamContent;
    
    return await _httpClient.SendAsync(proxyRequest, HttpCompletionOption.ResponseContentRead);
  }
}