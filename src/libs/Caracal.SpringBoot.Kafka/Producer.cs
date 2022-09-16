using System.Net;
using System.Text.Json;
using Caracal.Web.Core.Messaging;
using Confluent.Kafka;

namespace Caracal.SpringBoot.Kafka;

public class Producer : IWriteOnlyQueue {
  private readonly ProducerConfig _config;

  public Producer() {
    _config = new ProducerConfig {
      BootstrapServers = "host.docker.internal:19092,localhost:9092",
      ClientId = Dns.GetHostName(),
    };
  }

  public Task<bool> PublishAsync<T>(string name, KeyValuePair<string, T> message, CancellationToken cancellationToken) {
    using var producer = new ProducerBuilder<string, string>(_config).Build();
    var messageString = JsonSerializer.Serialize(message.Value);
    var msg = new Message<string, string> {Key = message.Key, Value = messageString};
    
     producer.Produce(name, msg, deliveryReport =>
     {
          if (deliveryReport.Error.Code != ErrorCode.NoError)
          {
              Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
          }
          else
          {
              Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
              //numProduced += 1;
          }
     });
     
    var outstanding = producer.Flush(TimeSpan.FromSeconds(10));

    return Task.FromResult(outstanding == 0); // || response.Status == PersistenceStatus.Persisted;
  }
}