-module(request_maker).
-export([send/1]).
-import(utils, [repeat/2]).

send(N) ->
    repeat(fun() ->
        ssl:start(),
        application:start(inets),
        httpc:request(post,
            {"http://localhost:8080/save-lyric", [],
            "application/json",
            "{'text':'Hello goto learn javascript and python.'}"
        }, [], [])
    end, N).
