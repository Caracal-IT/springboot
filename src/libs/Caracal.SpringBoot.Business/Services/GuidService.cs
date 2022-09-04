using Caracal.SpringBoot.Business.Repositories;

namespace Caracal.SpringBoot.Business.Services;

public class GuidService : IGuidService, IInjectable {
  public GuidService(IGuidRepository repository) {
    Id = repository.GetNewId();
  }

  public Guid Id { get; set; }
}