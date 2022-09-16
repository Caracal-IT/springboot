using System.Text.Json;
using Confluent.Kafka;
using Caracal.Web.Core.Messaging;

namespace Caracal.SpringBoot.Kafka;

public class Consumer : IReadonlyQueue {
    private readonly Dictionary<string, string> _config;

    public Consumer() {
        _config = new Dictionary<string, string> {
            {"bootstrap.servers", "host.docker.internal:9092, localhost:9092"},
            {"group.id", "kafka-dotnet-getting-started"},
            {"auto.offset.reset", "earliest"}
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