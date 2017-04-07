using System;
using RabbitMQClient;
using CommunicationModel;
using LyricNotifications;
using LyricModel;

namespace SagaManager
{
    class Manager
    {
        static void Main(string[] args)
        {
            Queue.SpawnConsumer(Queue.SAGA_MANAGER_QUEUE, (message) => {
                handleMessage(message);
                return 0;
            });

            Console.WriteLine("Bad words remover consuming.");
        }

        private static void handleMessage(string message)
        {
            var response = Command.deserializeJson(message);
            var lyric = Lyric.deserializeJson(response.result);
            if (Command.checkCommandResultSuccess(response))
            {
                handleCommandSuccess(response, lyric);
            }
            else
            {
                handleCommandError(response, lyric);
            }
        }

        private static void handleCommandSuccess(Command.ServiceResponse response, Lyric.Lyric lyric)
        {
            switch (response.sender)
            {
                case Command.CommandSender.Router:
                    Queue.publishMessage(Queue.REMOVE_BAD_WORDS_QUEUE, response.result);
                    Events.handleUploadInitialized(lyric);
                    break;
                case Command.CommandSender.BadWordsReplacer:
                    Queue.publishMessage(Queue.LOWERCASE_CAPS_WORDS_QUEUE, response.result);
                    break;
                case Command.CommandSender.CapsWordsToLowerCaser:
                    Queue.publishMessage(Queue.LYRIC_VALIDATOR_QUEUE, response.result);
                    break;
                case Command.CommandSender.Validator:
                    Queue.publishMessage(Queue.STORE_LYRIC_QUEUE, response.result);
                    break;
                case Command.CommandSender.Storage:
                    Events.handleUploadCompleted(lyric);
                    break;
            }
        }

        private static void handleCommandError(Command.ServiceResponse response, Lyric.Lyric lyric)
        {
            switch (response.sender)
            {
                case Command.CommandSender.Validator:
                    Events.handleUploadFailed(lyric);
                    break;
            }
        }
    }
}
