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
    let SAVE_NOTE_ROUTE = "/save-note"
    [<Literal>]
    let SLEEP_MILLISECONDS = 2000

type Storage() =
    let redis = new RedisClient(Config.REDIS_HOST)
    member this.set key value = redis.Set(key, value)

module Notes =
    type Note = { 
        text : string 
        mutable id : string
    }

    let serializeId note =
        sprintf "{\"id\":\"%s\"}" note.id

    let generateId =
        Guid.NewGuid().ToString()

    let store note =
        note.id <- generateId
        let storage = new Storage()
        storage.set note.id (UTF8.bytes note.text)
        note

    let parseFromRequest request = 
        try
            let fromJson n = JsonConvert.DeserializeObject<Note> n
            request.rawForm |> UTF8.toString |> fromJson
        with
            _ -> failwith "Invalid post data"

    let validate note =
        if note.text = null then
            failwith "Empty note text"
        note

let saveNote request = 
    try
        request |> Notes.parseFromRequest |> Notes.validate |> Notes.store |> Notes.serializeId |> OK
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
        POST >=> path Config.SAVE_NOTE_ROUTE >=> (sleep Config.SLEEP_MILLISECONDS) >=> request saveNote
        NOT_FOUND "Resource not found"
    ]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig app
    0