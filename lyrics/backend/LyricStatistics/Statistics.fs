open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open System
open System.Text
open System.Threading
open RabbitMQClient
open LyricModel

module Config =
    [<Literal>]
    let GET_STATISTICS_ROUTE = "/statistics"
    [<Literal>]
    let HOST = "127.0.0.1"
    [<Literal>]
    let PORT = 8081

type Cache() =
    static let mutable cache : string list = []
    static let _lock = new Object()

    static member addMessage msg =
        lock _lock (fun () ->
            cache <- (List.append cache [msg])
        )

    static member getMessages =
        cache

let consumer message =
    Cache.addMessage message
    1

let spawnConsumerAsync =
    Async.Start(async {
       Queue.SpawnConsumer(Queue.STATISTICS_NOTIFICATIONS_QUEUE, Func<string, int>(consumer))
    })

let getStatistics request =
    try
        let messagesStr = Cache.getMessages |> List.toSeq<string> |> String.concat ","
        (sprintf "[%s]" messagesStr) |> OK
    with
        Failure msg -> BAD_REQUEST msg

let app : WebPart =
    choose [
        GET >=> path Config.GET_STATISTICS_ROUTE >=> request getStatistics
        NOT_FOUND "Resource not found"
    ]

[<EntryPoint>]
let main _ =
    spawnConsumerAsync

    let conf = { defaultConfig with bindings = [ HttpBinding.createSimple HTTP Config.HOST Config.PORT ] }
    startWebServer conf app
    0
