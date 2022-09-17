namespace Caracal.SpringBoot.Kafka;

public class Consumer : IReadonlyQueue {
    private const string GroupIdPrefix = "spring-boot-";
    
    private readonly string _bootstrapServers;
    private readonly ILogger<Consumer> _logger;
    
    public Consumer(ILogger<Consumer> logger, string? bootstrapServers) {
        _logger = logger;
        _bootstrapServers = bootstrapServers ?? "localhost:9092";
    }

    public IEnumerable<KeyValuePair<string, T>> Subscribe<T>(string topic, CancellationToken cancellationToken) {
        using var consumer = CreateConsumer(topic);

        while (!cancellationToken.IsCancellationRequested) {
            var result = TryConsume(consumer, cancellationToken);

            if (result?.Message?.Value == null)
                continue;

            var message = Deserialize<T>(result.Message.Value);

            if (message != null)
                yield return new KeyValuePair<string, T>(result.Message.Key, message);
        }

        consumer.Close();
    }

    private IConsumer<string, string> CreateConsumer(string topic) {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = $"{GroupIdPrefix}{topic.ToLower()}",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);

        return consumer;
    }

    private ConsumeResult<string, string>? TryConsume(IConsumer<string, string> consumer, CancellationToken cancellationToken) {
        try {
            return consumer.Consume(cancellationToken);
        }
        catch (OperationCanceledException) {
            return null;
        }
        catch (Exception exception) {
            _logger.LogError(exception, "An unexpected exception occured");
            return null;
        }
    }
}