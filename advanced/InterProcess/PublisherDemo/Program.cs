using System;
using System.Threading.Tasks;
using Cloudtoid.Interprocess;

namespace Publisher
{
    internal class Program
    {
        internal static async Task Main()
        {
            // Create the queue factory. If you are not interested in tracing the internals of
            // the queue then don't pass in a loggerFactory

            var factory = new QueueFactory();

            // Create a message queue publisher

            var options = new QueueOptions(
                queueName: "sample-queue",
                bytesCapacity: 1024 * 1024);

            using var publisher = factory.CreatePublisher(options);
            using var subscriber = factory.CreateSubscriber(options);
            var messageBuffer = new byte[1];

            // Enqueue messages

            for (byte i = 0; i < 255;)
            {
                Console.Write("Enqueue: ");
                Console.WriteLine(i);

                if (publisher.TryEnqueue(new byte[] { i }))
                    i++;

                if (subscriber.TryDequeue(messageBuffer, default, out var message))
                {
                    Console.Write("Dequeue: ");
                    Console.WriteLine(messageBuffer[0]);
                }

                await Task.Delay(2000);
            }
        }
    }
}