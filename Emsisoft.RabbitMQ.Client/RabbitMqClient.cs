using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Emsisoft.RabbitMQ.Client
{
    public class RabbitMqClient
    {
        private const string queueName = "Emsisoft";
        public static void Send(IEnumerable<byte[]> hashesBatch)
        {
            GetChannel(out IConnection connection, out IModel channel);
            using (connection)
            using (channel)
            {
                hashesBatch.ToList().ForEach(hashBytes =>
                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: null,
                                         body: hashBytes)
                );
            }
        }

        public static void Receive(EventHandler<BasicDeliverEventArgs> handler)
        {
            GetChannel(out IConnection connection, out IModel channel);

            channel.BasicQos(prefetchSize: 100, prefetchCount: 1, global: false); //100 hashes at a time to avoid extra DB calls

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += handler;
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private static void GetChannel(out IConnection connection, out IModel channel)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }
    }
}