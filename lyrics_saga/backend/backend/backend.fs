open Suave
open Suave.Web
open Suave.Json
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open System
open System.Text
open SagaManager
open LyricModel

module Config =
    [<Literal>]
    let SAVE_LYRIC_ROUTE = "/save-lyric"

let sendForProcessing lyric =
    Dispatcher.handleNewLyric(lyric)
    lyric

let parseFromRequest request = 
    try
        request.rawForm |> UTF8.toString |> Lyric.deserializeJson
    with
        _ -> failwith "Invalid post data"

let saveNote request = 
    try
        request
            |> parseFromRequest
            |> Lyric.validate
            |> Lyric.generateId
            |> sendForProcessing
            |> Lyric.serializeId
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