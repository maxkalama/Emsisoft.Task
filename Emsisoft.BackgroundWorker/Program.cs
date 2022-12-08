using Emsisoft.RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

RabbitMqClient.Receive(handler);

Console.WriteLine("Press Enter to exit");
Console.ReadLine();



static void handler(object? model, BasicDeliverEventArgs ea)
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message);
}


