using Caracal.Web.Core.Messaging;
using Confluent.Kafka;

namespace Caracal.SpringBoot.Kafka;

public class Producer: IWriteOnlyQueue {
  public bool Publish<T>(string name, T message, CancellationToken cancellationToken) {
    return true;
  }
  public void Send() {
    var clientConfig = new ClientConfig();
    clientConfig.BootstrapServers = "localhost:9092";
    clientConfig.SecurityProtocol = Confluent.Kafka.SecurityProtocol.SaslPlaintext;
    clientConfig.SaslMechanism = Confluent.Kafka.SaslMechanism.Plain;
    //clientConfig.SaslUsername="<api-key>";
    //clientConfig.SaslPassword="<api-secret>";
    //clientConfig.SslCaLocation = "probe"; // /etc/ssl/certs
    //await Produce("recent_changes", clientConfig);
    //Consume("recent_changes", clientConfig);
    const string topic = "withdrawals";

    string[] users = {"eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther"};
    string[] items = {"book", "alarm clock", "t-shirts", "gift card", "batteries"};
    Random rnd = new Random();

    using (var producer = new ProducerBuilder<string, string>(clientConfig).Build()) {
      var numProduced = 0;
      const int numMessages = 10;
      for (int i = 0; i < numMessages; ++i) {
        var user = users[rnd.Next(users.Length)];
        var item = items[rnd.Next(items.Length)];

        producer.Produce(topic, new Message<string, string> {Key = user, Value = item},
          (deliveryReport) => {
            if (deliveryReport.Error.Code != ErrorCode.NoError) {
              Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
            }
            else {
              Console.WriteLine($"Produced event to topic {topic}: key = {user,-10} value = {item}");
              numProduced += 1;
            }
          });
      }

      producer.Flush(TimeSpan.FromSeconds(10));
      Console.WriteLine($"{numProduced} messages were produced to topic {topic}");

    }
  }


  public async Task SendAsync(string topic, CancellationToken cancellationToken) {
    //var clientConfig = new ClientConfig {
    //  BootstrapServers = "localhost:9092"
    //};

    var config = new Dictionary<string, string>();
    config.Add("bootstrap.servers", "host.docker.internal:9092, localhost:9092");
    
    
    /*
     bootstrap.servers=
security.protocol=SASL_SSL
sasl.mechanisms=PLAIN
sasl.username=< CLUSTER API KEY >
sasl.password=< CLUSTER API SECRET > 
     */

    string[] users = {"eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther"};
    string[] items = {"book", "alarm clock", "t-shirts", "gift card", "batteries"};
    Random rnd = new Random();

    using var producer = new ProducerBuilder<string, string>(config).Build();

    var numProduced = 0;
    const int numMessages = 10;
    for (int i = 0; i < numMessages; ++i) {
      var user = users[rnd.Next(users.Length)];
      var item = items[rnd.Next(items.Length)];
      
      var response = await producer.ProduceAsync(topic, new Message<string, string> {Key = user, Value = item}, cancellationToken);
    }

    producer.Flush(TimeSpan.FromSeconds(10));
    Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
  }
}

/*
class Producer2 {
  static void Main(string[] args)
  {
    if (args.Length != 1) {
      Console.WriteLine("Please provide the configuration file path as a command line argument");
    }

    IConfiguration configuration = new ConfigurationBuilder()
      .AddIniFile(args[0])
      .Build();

    const string topic = "purchases";

    string[] users = { "eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther" };
    string[] items = { "book", "alarm clock", "t-shirts", "gift card", "batteries" };

    using (var producer = new ProducerBuilder<string, string>(
             configuration.AsEnumerable()).Build())
    {
      var numProduced = 0;
      readonly Random rnd = new Random();
      const int numMessages = 10;
      for (int i = 0; i < numMessages; ++i)
      {
        var user = users[rnd.Next(users.Length)];
        var item = items[rnd.Next(items.Length)];

        producer.Produce(topic, new Message<string, string> { Key = user, Value = item },
          (deliveryReport) =>
          {
            if (deliveryReport.Error.Code != ErrorCode.NoError) {
              Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
            }
            else {
              Console.WriteLine($"Produced event to topic {topic}: key = {user,-10} value = {item}");
              numProduced += 1;
            }
          });
      }

      producer.Flush(TimeSpan.FromSeconds(10));
      Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
    }
  }
}
*/
