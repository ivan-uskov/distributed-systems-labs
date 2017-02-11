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

module Config = 
    [<Literal>]
    let REDIS_HOST = "127.0.0.1:6379"
    [<Literal>]
    let SAVE_LYRIC_ROUTE = "/save-lyric"
    [<Literal>]
    let SLEEP_MILLISECONDS = 2000

type Storage() =
    let redis = new RedisClient(Config.REDIS_HOST)
    member this.set key value = redis.Set(key, value)

module Lyrics =
    type Lyric = { 
        text : string 
        mutable id : string
    }

    let serializeId lyric =
        sprintf "{\"id\":\"%s\"}" lyric.id

    let generateId =
        Guid.NewGuid().ToString()

    let store lyric =
        lyric.id <- generateId
        let storage = new Storage()
        storage.set lyric.id (UTF8.bytes lyric.text)
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
        request |> Lyrics.parseFromRequest |> Lyrics.validate |> Lyrics.store |> Lyrics.serializeId |> OK
    with
        Failure msg -> BAD_REQUEST msg

let sleep milliseconds: WebPart =
    fun (x : HttpContext) ->
        async {
            do! Async.Sleep milliseconds
            return! OK "end sleep" x
        }

let app : WebPart =
    choose [
        POST >=> path Config.SAVE_LYRIC_ROUTE >=> (sleep Config.SLEEP_MILLISECONDS) >=> request saveNote
        NOT_FOUND "Resource not found"
    ]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig app
    0