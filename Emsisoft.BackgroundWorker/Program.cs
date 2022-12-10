using Emsisoft.Data;
using Emsisoft.DB;
using Emsisoft.HashesService;
using Emsisoft.RabbitMQ.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine($"Hello, World! Starting messages recieving... Main thread {Thread.CurrentThread.ManagedThreadId}");

const int threadCount = 4;

ThreadPool.GetMaxThreads(out int maxWorker, out int maxCompletionThreads);
ThreadPool.SetMaxThreads(threadCount, maxCompletionThreads); 

for (int i = 0; i < threadCount; i++)
{
    var t = new Thread(() => ExecuteMessageConsumer(threadNumber: i));
    t.Start();
}

Console.WriteLine("Press Enter to exit");
Console.ReadLine();

void ExecuteMessageConsumer(int threadNumber)
{
    var service = new Sha1HashesService();
    var dbService = new DbHashService();

    const int dbBatchSize = 100;
    var dbBatch = new Hash[dbBatchSize];
    var ackBatch = new ulong[dbBatchSize];
    ushort current = 0;

    RabbitMqClient.GetChannel(out IConnection connection, out IModel channel);
    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.Received += async (model, ea) =>
    {
        current = await MessageHandlerAsync(model,
            ea,
            service,
            dbService,
            dbBatch,
            ackBatch,
            channel,
            current,
            threadNumber);
    };

    RabbitMqClient.StartConsuming(channel, consumer);  
}

async Task<ushort> MessageHandlerAsync(object? model,
    BasicDeliverEventArgs ea,
    IHashesService service,
    IDbHashService dbService,
    Hash[] dbBatch,
    ulong[] ackBatch,
    IModel channel,
    ushort current,
    int threadNumber)
{
    var body = ea.Body.ToArray();
    var hash = service.FromBinary(body);
    

    dbBatch[current] = new Hash { Hash = hash.Hash, Date = hash.Date };
    ackBatch[current] = ea.DeliveryTag;

    if (current >= 99) //99 is zero based 100 items
    {
        if (await dbService.TryInsertAsync(dbBatch))
        {
            ackBatch.ToList().ForEach(a => channel.BasicAck(a, multiple: false)); //ack the messages
            Console.WriteLine($" # Thread id:{Thread.CurrentThread.ManagedThreadId} num:{threadNumber} wrote {current+1} hashes"); //+1 since zero based
            current = 0;
        }
        else
        {
            throw new Exception("TryInsert failed.");
        }
    }
    else
        current++;

    return current;
}



