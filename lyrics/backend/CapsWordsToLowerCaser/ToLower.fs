open RabbitMQClient
open System.Text.RegularExpressions

let consumer message = 
    let msg = Regex.Replace(message, "([A-Z]+)", (fun (x : Match) -> x.Value.ToLower()))
    Queue.publishMessage(Queue.LYRIC_VALIDATOR_QUEUE, msg)
    0

[<EntryPoint>]
let main argv =
    Queue.SpawnConsumer(Queue.LOWERCASE_CAPS_WORDS_QUEUE, System.Func<string, int>(consumer))
    printfn "Caps remover consuming..."
    0
