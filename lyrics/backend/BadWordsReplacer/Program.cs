using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BadWordsReplacer
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue.SpawnConsumer((body) => {
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
                Console.WriteLine(" [x] Done");

                return 0;
            });

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
