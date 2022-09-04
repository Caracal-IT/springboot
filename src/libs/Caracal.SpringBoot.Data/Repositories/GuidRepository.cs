using Caracal.SpringBoot.Business.Repositories;

namespace Caracal.SpringBoot.Data.Repositories;

public class GuidRepository : IGuidRepository, IScoped {
  public Guid GetNewId() {
    return Guid.NewGuid();
  }
}