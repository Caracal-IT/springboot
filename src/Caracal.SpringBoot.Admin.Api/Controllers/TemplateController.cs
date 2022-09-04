using Caracal.SpringBoot.Templates.Services;
using Microsoft.Extensions.FileProviders;

namespace Caracal.SpringBoot.Admin.Api.Controllers; 

[ApiController]
[Route("[controller]")]
public class TemplateController: ControllerBase {
    private IFileProvider _fileProvider;
    private IHostEnvironment _environment;
    private readonly ITemplateService _templateService;
    
    public TemplateController(IHostEnvironment environment, ITemplateService templateService) {
        _templateService = templateService;
        _fileProvider = environment.ContentRootFileProvider;
        _environment = environment;
    }

    [HttpGet("default")]
    public async Task<ObjectResult> GetDefaultTemplate(CancellationToken cancellationToken) {
        var relativePaths = new[] {"bin", "/bin", "~/bin"};
        var physicalPaths = _templateService.GetPhysicalPaths(relativePaths);
        
        await Task.Delay(10, cancellationToken);
        var fileInfo = _fileProvider.GetFileInfo("bin");
        
        return Ok($"Template Found: {fileInfo.PhysicalPath} Physical Paths = {string.Join(",\n", physicalPaths)}");
    }
    
}