namespace CommunicationModel

open System
open Newtonsoft.Json
open Newtonsoft.Json.Converters;
open LyricModel

module Command =
    module CommandSender =
        [<Literal>]
        let Router = 0
        [<Literal>]
        let BadWordsReplacer = 1
        [<Literal>]
        let CapsWordsToLowerCaser = 2
        [<Literal>]
        let Validator = 3
        [<Literal>]
        let Storage = 4

    module CommandStatus =
        [<Literal>]
        let Success = 1
        [<Literal>]
        let Error = 0

    type ServiceResponse = {
        mutable result : string
        mutable status : int
        mutable sender : int
    }

    let prepareServiceResponse(lyric : Lyric.Lyric, sender : int, status : int) =
        let lyricText = Lyric.serializeJson lyric
        {result = lyricText; status = status; sender = sender}

    let serializeJson response =
        JsonConvert.SerializeObject response

    let deserializeJson encoded = 
        try
            JsonConvert.DeserializeObject<ServiceResponse> encoded
        with
            _ -> failwith "Invalid post data"

    let checkCommandResultSuccess result =
        result.status = CommandStatus.Success