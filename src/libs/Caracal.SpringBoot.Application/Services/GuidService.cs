using Caracal.SpringBoot.Application.Repositories;

namespace Caracal.SpringBoot.Application.Services;

public sealed class GuidService : IGuidService, IScopedInjectable {
  public GuidService(IGuidRepository repository) =>
    Id = repository.GetNewId();

  public Guid Id { get; set; }
}