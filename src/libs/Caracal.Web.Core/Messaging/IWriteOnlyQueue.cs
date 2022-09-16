namespace Caracal.Web.Core.Messaging; 

public interface IWriteOnlyQueue {
  Task<bool> PublishAsync<T>(string name, KeyValuePair<string, T> message, CancellationToken cancellationToken);
}