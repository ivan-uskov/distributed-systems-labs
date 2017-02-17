namespace LyricModel

open System
open Newtonsoft.Json

module Lyric =
    type Lyric = {
        text : string
        mutable id : string
    }

    let serializeId lyric =
        sprintf "{\"id\":\"%s\"}" lyric.id

    let serializeJson lyric =
        JsonConvert.SerializeObject lyric

    let deserializeJson encoded = 
        try
            JsonConvert.DeserializeObject<Lyric> encoded
        with
            _ -> failwith "Invalid post data"

    let generateId lyric =
        lyric.id <- Guid.NewGuid().ToString()
        lyric

    let validate lyric =
        if lyric.text = null then
            failwith "Empty lyric text"
        lyric