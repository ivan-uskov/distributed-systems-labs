using System;
using System.Text;
using RabbitMQClient;
using RedisClient;
using LyricModel;

namespace LyricSaver
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue.SpawnConsumer(Queue.STORE_LYRIC_QUEUE, (message) => {
                var lyric = Lyric.deserializeJson(message);
                Lyric.validate(lyric);
                var lyricText = Encoding.UTF8.GetBytes(lyric.text);
                Storage.store(lyric.id, lyricText);
                return 0;
            });

            Console.WriteLine("LyricSaver consuming...");
        }
    }
}
