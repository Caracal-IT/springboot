namespace Caracal.Web.Core.Messaging; 

public interface IReadonlyQueue {
  IEnumerable<KeyValuePair<string, T>> Subscribe<T>(string name, CancellationToken cancellationToken);
}