using Microsoft.Extensions.FileProviders;

namespace Caracal.SpringBoot.Templates.Services;

public interface ITemplateService {
  IEnumerable<string> GetPhysicalPaths(IEnumerable<string> locations);
}

public class TemplateService : ITemplateService {
  private readonly IFileProvider _fileProvider;

  public TemplateService(IFileProvider fileProvider) {
    _fileProvider = fileProvider;
  }

  public IEnumerable<string> GetPhysicalPaths(IEnumerable<string> locations) {
    return locations.Select(GetFullPath)
      .Where(p => !string.IsNullOrWhiteSpace(p));
  }

  private string GetFullPath(string relativePath) {
    var directoryContents = _fileProvider.GetDirectoryContents(relativePath);

    return directoryContents.Exists ? directoryContents.First().PhysicalPath : string.Empty;
  }
}

public class TemplateService2 : ITemplateService {
  private readonly IFileProvider _fileProvider;

  public TemplateService2(IFileProvider fileProvider) {
    _fileProvider = fileProvider;
  }

  public IEnumerable<string> GetPhysicalPaths(IEnumerable<string> locations) {
    return locations.Select(GetFullPath)
      .Where(p => !string.IsNullOrWhiteSpace(p));
  }

  private string GetFullPath(string relativePath) {
    var directoryContents = _fileProvider.GetDirectoryContents(relativePath);

    return directoryContents.Exists ? directoryContents.First().PhysicalPath : string.Empty;
  }
}