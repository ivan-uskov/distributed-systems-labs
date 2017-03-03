-module(utils).
-export([repeat/2]).

repeat(F, N) when N == 1 -> F();
repeat(F, N) when N > 1  -> F(), repeat(F, N - 1).