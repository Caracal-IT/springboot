using Caracal.SpringBoot.Templates.Services;
using Microsoft.Extensions.FileProviders;

namespace Caracal.SpringBoot.Admin.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class TemplateController : ControllerBase {
  private readonly ITemplateService _templateService;
  private readonly IFileProvider _fileProvider;
  private readonly ILogger<TemplateController> _logger;

  public TemplateController(IHostEnvironment environment, ITemplateService templateService, ILogger<TemplateController> logger) {
    _templateService = templateService;
    _logger = logger;
    _fileProvider = environment.ContentRootFileProvider;
  }

  [HttpGet("default")]
  public async Task<ObjectResult> GetDefaultTemplate(CancellationToken cancellationToken) {
    var relativePaths = new[] {"bin", "/bin", "~/bin"};
    var physicalPaths = _templateService.GetPhysicalPaths(relativePaths);

    await Task.Delay(10, cancellationToken);
    var fileInfo = _fileProvider.GetFileInfo("bin");

    return Ok($"Template Found: {fileInfo.PhysicalPath} Physical Paths = {string.Join(",\n", physicalPaths)}");
  }

  [HttpGet("embedded")]
  public async Task<OkObjectResult> ReadEmbedded(CancellationToken cancellationToken) {
    var provider = new EmbeddedFileProvider(typeof(ITemplateService).Assembly);

    var fileInfo = provider.GetFileInfo("Templates/EmbeddedDefaultTemplate.xml");

    await using var stream = fileInfo.CreateReadStream(); 
    using var reader = new StreamReader(stream);
    var content = await reader.ReadToEndAsync(cancellationToken);
    
    _logger.LogWarning($"Loading embedded template");
    
    return Ok(content);
  }
}