using System;
using RabbitMQClient;
using LyricModel;
using SagaManager;

namespace LyricValidator
{
    class Validator
    {
        static void Main(string[] args)
        {
            Queue.SpawnConsumer(Queue.LYRIC_VALIDATOR_QUEUE, (message) => {
                Lyric.Lyric lyric = Lyric.deserializeJson(message);
                if (lyric.text.Length == 0)
                {
                    Dispatcher.handleValidatorError(lyric);
                }
                else
                {
                    Dispatcher.handleValidatorSuccess(lyric);
                }
                return 0;
            });

            Console.WriteLine("Bad words remover consuming.");
        }
    }
}
