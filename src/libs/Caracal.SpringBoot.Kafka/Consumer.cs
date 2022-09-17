using System.Text.Json;
using Confluent.Kafka;
using Caracal.Web.Core.Messaging;
using Microsoft.Extensions.Logging;

namespace Caracal.SpringBoot.Kafka;

public class Consumer : IReadonlyQueue {
    private readonly ILogger<Consumer> _logger;
    private readonly string _bootstrapServers;
    private readonly string _groupIdPrefix;

    public Consumer(ILogger<Consumer> logger) {
        _logger = logger;
        
        _bootstrapServers = "host.docker.internal:19092,localhost:9092";
        _groupIdPrefix = "spring-boot-";
    }

    public IEnumerable<KeyValuePair<string, T>> Subscribe<T>(string topic, CancellationToken cancellationToken) {
        using var consumer = CreateConsumer(topic);

        while (!cancellationToken.IsCancellationRequested) {
            var result = TryConsume(consumer, cancellationToken);

            if (result?.Message?.Value == null)
                continue;

            var message = JsonSerializer.Deserialize<T>(result.Message.Value);

            if (message != null)
                yield return new KeyValuePair<string, T>(result.Message.Key, message);
        }

        consumer.Close();
    }

    private IConsumer<string, string> CreateConsumer(string topic) {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = $"{_groupIdPrefix}{topic.ToLower()}",
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
            Console.WriteLine("Operation Canceled");
            return null;
        }
        catch (Exception exception) {
            _logger.LogError(exception, "An unexpected exception occured");
            return null;
        }
    }
}