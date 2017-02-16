open Suave
open Suave.Web
open Suave.Json
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Newtonsoft.Json
open System
open ServiceStack.Redis
open RabbitMQ.Client
open System.Text

module Config = 
    [<Literal>]
    let REDIS_HOST = "127.0.0.1:6379"
    [<Literal>]
    let RABBIT_HOST = "localhost"
    [<Literal>]
    let SAVE_LYRIC_ROUTE = "/save-lyric"
    [<Literal>]
    let SLEEP_MILLISECONDS = 2000
    [<Literal>]
    let NEXT_STEP_QUEUE = "remove_bad_words_queue"

type Storage() =
    let redis = new RedisClient(Config.REDIS_HOST)
    member this.set key value = redis.Set(key, value)

module Queue =
    [<Literal>]
    let private QUEUE_NAME = "remove_bad_words_queue"
    [<Literal>]
    let private EXCHANGE_NAME = "remove_bad_words_queue_exchange"
    [<Literal>]
    let private EXCHANGE_TYPE = "direct"
    [<Literal>]
    let private ROUTING_NAME = "remove_bad_words_queue_routing"

    let publish msg =
        let factory = new ConnectionFactory(HostName = Config.RABBIT_HOST)
        let connection = factory.CreateConnection()
        let channel = connection.CreateModel()
        channel.QueueDeclare(QUEUE_NAME, false, false, false, null) |> ignore
        channel.ExchangeDeclare(EXCHANGE_NAME, EXCHANGE_TYPE, false, false, null) |> ignore
        channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, ROUTING_NAME, null) |> ignore

        channel.BasicPublish(EXCHANGE_NAME, ROUTING_NAME, null, (UTF8.bytes msg))

module Lyrics =
    type Lyric = { 
        text : string 
        mutable id : string
    }

    let serializeId lyric =
        sprintf "{\"id\":\"%s\"}" lyric.id

    let generateId lyric =
        lyric.id <- Guid.NewGuid().ToString()
        lyric

    let sendForProcessing lyric =
        let msg = JsonConvert.SerializeObject lyric
        printfn "%s" msg
        Queue.publish msg
        lyric

    let parseFromRequest request = 
        try
            let fromJson n = JsonConvert.DeserializeObject<Lyric> n
            request.rawForm |> UTF8.toString |> fromJson
        with
            _ -> failwith "Invalid post data"

    let validate note =
        if note.text = null then
            failwith "Empty lyric text"
        note

let saveNote request = 
    try
        request
            |> Lyrics.parseFromRequest 
            |> Lyrics.validate 
            |> Lyrics.generateId 
            |> Lyrics.sendForProcessing 
            |> Lyrics.serializeId 
            |> OK
    with
        Failure msg -> BAD_REQUEST msg

let app : WebPart =
    choose [
        POST >=> path Config.SAVE_LYRIC_ROUTE >=> request saveNote
        NOT_FOUND "Resource not found"
    ]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig app
    0