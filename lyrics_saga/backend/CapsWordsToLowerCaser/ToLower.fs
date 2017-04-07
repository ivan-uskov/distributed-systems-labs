open RabbitMQClient
open System.Text.RegularExpressions
open LyricModel
open SagaManager

let consumer message =
    let lyric = Lyric.deserializeJson message
    lyric.text <- Regex.Replace(lyric.text, "([A-Z]+)", (fun (x : Match) -> x.Value.ToLower()))
    Dispatcher.handleCapsWordsToLowerCased lyric
    0

[<EntryPoint>]
let main argv =
    Queue.SpawnConsumer(Queue.LOWERCASE_CAPS_WORDS_QUEUE, System.Func<string, int>(consumer))
    printfn "Caps remover consuming..."
    0
