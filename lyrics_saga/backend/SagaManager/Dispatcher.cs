using LyricModel;
using RabbitMQClient;
using CommunicationModel;

namespace SagaManager
{
    public class Dispatcher
    {
        public static void handleNewLyric(Lyric.Lyric lyric)
        {
            handle(lyric, Command.CommandSender.Router);
        }

        public static void handleBadWordsReplaced(Lyric.Lyric lyric)
        {
            handle(lyric, Command.CommandSender.BadWordsReplacer);
        }

        public static void handleCapsWordsToLowerCased(Lyric.Lyric lyric)
        {
            handle(lyric, Command.CommandSender.CapsWordsToLowerCaser);
        }

        public static void handleValidatorError(Lyric.Lyric lyric)
        {
            handle(lyric, Command.CommandSender.Validator, Command.CommandStatus.Error);
        }

        public static void handleValidatorSuccess(Lyric.Lyric lyric)
        {
            handle(lyric, Command.CommandSender.Validator);
        }

        public static void handleLyricStored(Lyric.Lyric lyric)
        {
            handle(lyric, Command.CommandSender.Storage);
        }

        private static void handle(Lyric.Lyric lyric, int sender, int status = Command.CommandStatus.Success)
        {
            var response = Command.prepareServiceResponse(lyric, sender, status);
            Queue.publishMessage(Queue.SAGA_MANAGER_QUEUE, Command.serializeJson(response));
        }
    }
}
