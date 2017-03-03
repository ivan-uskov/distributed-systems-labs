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
            "{'text':'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa.'}"
        }, [], [])
    end, N).
