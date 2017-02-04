open Suave
open Suave.Web
open Suave.Json
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Newtonsoft.Json

module Notes =
    type Note =
        {
            text : string
            id   : string
        }

    let serialize = JsonConvert.SerializeObject
    let deserialize s = JsonConvert.DeserializeObject<Note> s

let saveNote request = 
    try
        let note = request.rawForm |> UTF8.toString |> Notes.deserialize

        if note.text = null then
            failwith "Empty note"

        note |> Notes.serialize |> OK
    with
        | Failure msg -> BAD_REQUEST "Note text not specified"
        | _ -> BAD_REQUEST "Invalid data"

let app : WebPart =
  choose
    [ POST >=> path "/save-note" >=> request saveNote ]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig app
    0