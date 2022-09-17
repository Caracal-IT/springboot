namespace Caracal.SpringBoot.Kafka;

public class Producer : IWriteOnlyQueue {
  private readonly ILogger<Producer> _logger;
  private readonly string _bootstrapServers;
  private readonly double _timeout;

  public Producer(ILogger<Producer> logger) {
    _logger = logger;
    _bootstrapServers = "host.docker.internal:19092,localhost:9092";
    _timeout = 10;
  }

  public Task<bool> PublishAsync<T>(string topic, KeyValuePair<string, T> message, CancellationToken cancellationToken) {
    ArgumentException.ThrowIfNullOrEmpty(topic);
    
    using var producer = CreateProducer();
    var msg = new Message<string, string> {Key = message.Key, Value = Serialize(message.Value)};
    
     producer.Produce(topic, msg, ProcessDeliveryReport);
     var outstanding = producer.Flush(TimeSpan.FromSeconds(_timeout));

    return Task.FromResult(outstanding == 0);
  }

  private IProducer<string, string> CreateProducer() {
    var config = new ProducerConfig {
      BootstrapServers = _bootstrapServers,
      ClientId = Dns.GetHostName(),
    };
    
    return new ProducerBuilder<string, string>(config).Build();
  }

  private void ProcessDeliveryReport(DeliveryReport<string, string> deliveryReport) {
    if (deliveryReport.Error.Code != ErrorCode.NoError)
      _logger.LogError(new EventId((int)deliveryReport.Error.Code, deliveryReport.Error.Reason), "Failed to publish to kafka");
    else
      _logger.LogInformation(new EventId(0, $"Produced message to: {deliveryReport.TopicPartitionOffset}"), "Produced message to kafka");
  }
}