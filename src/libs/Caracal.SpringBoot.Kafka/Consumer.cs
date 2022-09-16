using System.Text.Json;
using Confluent.Kafka;
using Caracal.Web.Core.Messaging;

namespace Caracal.SpringBoot.Kafka;

public class Consumer : IReadonlyQueue {
    private readonly ConsumerConfig _config;

    public Consumer() {
        _config = new ConsumerConfig
        {
            BootstrapServers = "host.docker.internal:19092,localhost:9092",
            GroupId = "kafka-dotnet-getting-started",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    public IEnumerable<KeyValuePair<string, T>> Subscribe<T>(string topic, CancellationToken cancellationToken) {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();
        consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested) {
            var result = TryConsume(consumer, cancellationToken);

            if (result?.Message?.Value == null || cancellationToken.IsCancellationRequested)
                continue;

            var message = JsonSerializer.Deserialize<T>(result.Message.Value);

            if (message != null)
                yield return new KeyValuePair<string, T>(result.Message.Key, message);
        }

        consumer.Close();
    }

    private static ConsumeResult<string, string>? TryConsume(IConsumer<string, string> consumer, CancellationToken cancellationToken) {
        try {
            return consumer.Consume(cancellationToken);
        }
        catch (OperationCanceledException) {
            Console.WriteLine("Operation Canceled");
            return null;
        }
    }
}