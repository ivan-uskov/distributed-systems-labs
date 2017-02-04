open Suave
open Suave.Web
open Suave.Json
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Newtonsoft.Json
open System

module Notes =
    type Note = { 
        text : string 
        mutable id : string
    }

    let serialize = JsonConvert.SerializeObject
    let deserialize n = JsonConvert.DeserializeObject<Note> n

let saveNote request = 
    try
        let note = request.rawForm |> UTF8.toString |> Notes.deserialize

        if note.text = null then
            failwith "Empty note"

        note.id <- Guid.NewGuid().ToString()

        note |> Notes.serialize |> OK
    with
        | Failure msg -> BAD_REQUEST "Note text not specified"
        | _ -> BAD_REQUEST "Invalid data"

let app : WebPart =
    choose [
        POST >=> path "/save-note" >=> request saveNote 
    ]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig app
    0