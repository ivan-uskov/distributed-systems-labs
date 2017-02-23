using System;
using RabbitMQClient;
using System.Collections.Generic;

namespace BadWordsReplacer
{
    class Program
    {
        static void Main(string[] args)
        {
            var replaceMap = createReplaceMap();
            Queue.SpawnConsumer(Queue.REMOVE_BAD_WORDS_QUEUE, (message) => {
                Queue.publishMessage(Queue.LOWERCASE_CAPS_WORDS_QUEUE, prepareString(replaceMap, message));
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
