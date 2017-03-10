open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open System
open System.Text
open System.Threading
open RabbitMQClient
open RedisClient
open LyricModel

module Config =
    [<Literal>]
    let GET_LYRIC_ROUTE = "/get-lyric"
    [<Literal>]
    let HOST = "127.0.0.1"
    [<Literal>]
    let PORT = 8082

let consumer message =
    let lyric = Lyric.deserializeJson message
    Storage.store lyric.id (Encoding.UTF8.GetBytes lyric.text)
    1

let spawnConsumerAsync =
    Async.Start(async {
       Queue.SpawnConsumer(Queue.STORE_LYRIC_QUEUE, System.Func<string, int>(consumer))
    })

let parseIdFromRequest request = 
    try
        request.rawForm |> UTF8.toString |> Lyric.deserializeJson
    with
        _ -> failwith "Invalid post data"

let getLyricFromStorage (lyric : Lyric.Lyric) =
    lyric.text <- Storage.get lyric.id
    if lyric.text = "" || lyric.text = null then
        failwith "Lyric not exists"
    lyric

let getLyric request = 
    try
        request
            |> parseIdFromRequest
            |> getLyricFromStorage
            |> Lyric.serializeJson
            |> OK
    with
        Failure msg -> BAD_REQUEST msg

let app : WebPart =
    choose [
        POST >=> path Config.GET_LYRIC_ROUTE >=> request getLyric
        NOT_FOUND "Resource not found"
    ]

[<EntryPoint>]
let main argv =
    

    let conf = { defaultConfig with bindings = [ HttpBinding.createSimple HTTP Config.HOST Config.PORT ] }
    startWebServer conf app
    0
