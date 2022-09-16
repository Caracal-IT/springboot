using System.Text.Json;
using Caracal.Web.Core.Messaging;
using Confluent.Kafka;

namespace Caracal.SpringBoot.Kafka;

public class Producer : IWriteOnlyQueue {
  private readonly Dictionary<string, string> _config;

  public Producer() {
    _config = new Dictionary<string, string> {
      {"bootstrap.servers", "host.docker.internal:9092, localhost:9092"},
      {"group.id", "kafka-dotnet-getting-started"},
      {"auto.offset.reset", "earliest"}
    };
  }

  public async Task<bool> PublishAsync<T>(string name, KeyValuePair<string, T> message, CancellationToken cancellationToken) {
    using var producer = new ProducerBuilder<string, string>(_config).Build();
    var messageString = JsonSerializer.Serialize(message.Value);

    var response = await producer.ProduceAsync(name, new Message<string, string> {Key = message.Key, Value = messageString}, cancellationToken);
    var outstanding = producer.Flush(TimeSpan.FromSeconds(10));

    return outstanding == 0 || response.Status == PersistenceStatus.Persisted;
  }
}