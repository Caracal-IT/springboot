namespace Caracal.SpringBoot.Data.Repositories;

//public sealed class GuidRepository : IGuidRepository, ITransientInjectable {
//public sealed class GuidRepository : IGuidRepository, IScopedInjectable {
public sealed class GuidRepository : IGuidRepository, ISingletonInjectable {
  private Guid _defaultGuid = Guid.Empty;
  private readonly object _theLock = new ();
  
  public Guid GetNewId() {
    if (_defaultGuid != Guid.Empty)
      return _defaultGuid;

    lock (_theLock) {
      if (_defaultGuid != Guid.Empty)
        return _defaultGuid;
      
      _defaultGuid = Guid.NewGuid();
    
      return _defaultGuid;
    }
  }
}