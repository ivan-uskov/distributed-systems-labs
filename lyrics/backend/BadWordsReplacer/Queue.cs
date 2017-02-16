using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BadWordsReplacer
{
    class Queue
    {
        public static void SpawnConsumer(Func<byte[], int> fn)
        {
            var factory = new ConnectionFactory() { HostName = Config.RABBIT_HOST };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: Config.COMSUMING_QUEUE,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                fn(body);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: Config.COMSUMING_QUEUE, noAck: false, consumer: consumer);
        }
    }
}
