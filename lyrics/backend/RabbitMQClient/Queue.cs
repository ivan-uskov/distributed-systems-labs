using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQClient
{
    public class Queue
    {
        public const string STATISTICS_NOTIFICATIONS_QUEUE = "statistics_notifications";
        public const string REMOVE_BAD_WORDS_QUEUE = "remove_bad_words";
        public const string LOWERCASE_CAPS_WORDS_QUEUE = "remove_caps_words";
        public const string LYRIC_VALIDATOR_QUEUE = "lyric_validator";
        public const string STORE_LYRIC_QUEUE = "store_lyric_words";

        private static IConnection connection;

        public static void SpawnConsumer(string queue, Func<string, int> fn)
        {
            var queueName = queue + Config.QUEUE_SUFFIX;
            var channel = getRabbitConnection().CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => {
                var body = ea.Body;
                try
                {
                    fn(Encoding.UTF8.GetString(body));
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch(Exception)
                {}
            };
            channel.BasicConsume(queue: queueName, noAck: false, consumer: consumer);
        }

        public static void publishMessage(string queue, string message)
        {
            var queueName = queue + Config.QUEUE_SUFFIX;
            var exchangeName = queue + Config.EXCHANGE_SUFFIX;
            var routingName = queue + Config.ROUTING_SUFFIX;

            var channel = getRabbitConnection().CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            channel.ExchangeDeclare(exchangeName, Config.EXCHANGE_TYPE, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingName, null);
            channel.BasicPublish(exchangeName, routingName, null, Encoding.UTF8.GetBytes(message));
        }

        private static IConnection getRabbitConnection()
        {
            if (connection == null)
            {
                var factory = new ConnectionFactory() { HostName = Config.RABBIT_HOST };
                connection = factory.CreateConnection();
            }

            return connection;
        }
    }
}
