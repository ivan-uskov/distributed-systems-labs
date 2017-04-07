using System;
using RabbitMQClient;
using LyricModel;
using System.Collections.Generic;
using SagaManager;

namespace BadWordsReplacer
{
    class BadRemover
    {
        static void Main(string[] args)
        {
            var replaceMap = createReplaceMap();
            Queue.SpawnConsumer(Queue.REMOVE_BAD_WORDS_QUEUE, (message) => {
                var lyric = Lyric.deserializeJson(message);
                lyric.text = prepareString(replaceMap, lyric.text);
                Dispatcher.handleBadWordsReplaced(lyric);

                return 0;
            });

            Console.WriteLine("Bad words remover consuming.");
        }

        private static Dictionary<string, string> createReplaceMap()
        {
            var map = new Dictionary<string, string>();
            map["javascript"] = "php";
            map["c#"] = "F#";
            map["python"] = "Go";
            map["goto"] = "";

            return map;
        }

        private static string prepareString(Dictionary<string, string> replaceMap, string str)
        {
            foreach (KeyValuePair<string, string> pair in replaceMap)
            {
                str = str.Replace(pair.Key, pair.Value);
            }

            return str;
        }
    }
}
