namespace Caracal.Web.Core.Messaging; 

public interface IWriteOnlyQueue {
  bool Publish<T>(string name, T message, CancellationToken cancellationToken);
}