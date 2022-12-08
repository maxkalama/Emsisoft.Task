using Emsisoft.HashesService;
using Emsisoft.RabbitMQ.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

var service = new Sha1HashesService();

RabbitMqClient.GetChannel(out IConnection connection, out IModel channel);
RabbitMqClient.StartConsuming(QueueMessageHandler, connection, channel);

Console.WriteLine("Press Enter to exit");
Console.ReadLine();


void QueueMessageHandler(object? model, BasicDeliverEventArgs ea)
{
    var body = ea.Body.ToArray();
    var hash = service.FromBinary(body);
    Console.WriteLine($" # Got hash {hash.Date} {hash.Hash}");
    channel.BasicAck(ea.DeliveryTag, multiple: false);
}



