-module(supervisord).
-export([run/1, run_consumer/1]).
-import(utils, [repeat/2]).
-import(string, [concat/2]).

wait_for_process_end(Port, Data) ->
    receive
        {Port, {data, NewData}} -> wait_for_process_end(Port, Data++NewData);
        {Port, {exit_status, 0}} -> Data;
        {Port, {exit_status, S}} -> throw({commandfailed, S})
    end.

prepare_program_path(Program) ->
    "backend/" ++ Program ++ "/bin/release/" ++ Program ++ ".exe".

run_consumer(Cmd) ->
    spawn(fun() ->
        Port = erlang:open_port(
            {spawn, prepare_program_path(Cmd)},
            [stream, exit_status, use_stdio, stderr_to_stdout, in, eof]
        ),
        wait_for_process_end(Port, [])
    end).

run_consumers(Cmd, N) ->
    repeat(fun()  ->
        run_consumer(Cmd)
    end, N).

run(N) ->
    run_consumer("backend"),
    run_consumers("BadWordsReplacer", N),
    run_consumers("CapsWordsToLowerCaser", N),
    run_consumers("LyricSaver", N).