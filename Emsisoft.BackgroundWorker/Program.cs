using Emsisoft.Data;
using Emsisoft.DB;
using Emsisoft.HashesService;
using Emsisoft.Models;
using Emsisoft.RabbitMQ.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

var service = new Sha1HashesService();
var dbService = new DbHashService();

const int dbBatchSize = 100;
var dbBatch = new Hash[dbBatchSize];
var ackBatch = new ulong[dbBatchSize];
ushort current = 0;

RabbitMqClient.GetChannel(out IConnection connection, out IModel channel);
RabbitMqClient.StartConsuming(QueueMessageHandler, connection, channel);

Console.WriteLine("Press Enter to exit");
Console.ReadLine();

void QueueMessageHandler(object? model, BasicDeliverEventArgs ea)
{
    var body = ea.Body.ToArray();
    var hash = service.FromBinary(body);
    

    dbBatch[current] = new Hash { Hash = hash.Hash, Date = hash.Date };
    ackBatch[current] = ea.DeliveryTag;

    if (current >= 99) //99 is zero based 100 items
    {
        if (dbService.TryInsert(dbBatch))
        {
            ackBatch.ToList().ForEach(a => channel.BasicAck(a, multiple: false)); //ack the messages
            Console.WriteLine($" # Wrote {current+1} hashes"); //+1 since zero based
            current = 0;
        }
        else
        {
            throw new Exception("TryInsert failed.");
        }
    }
    else
        current++;
}



