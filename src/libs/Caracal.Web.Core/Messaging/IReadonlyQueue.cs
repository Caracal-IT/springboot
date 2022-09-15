namespace Caracal.Web.Core.Messaging; 

public interface IReadonlyQueue {
  IEnumerable<T> Subscribe<T>(string name, CancellationToken cancellationToken);
}