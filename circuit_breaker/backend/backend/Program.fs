open Suave
open Suave.Web
open Suave.Json
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Suave.ServerErrors
open System
open System.Threading
open System.Diagnostics

module Config = 
    [<Literal>]
    let CALC_PI_ROUTE = "/calc-pi"
    [<Literal>]
    let TIMEOUT = 3000

type PiCalculator () = 
    static let _lock = new Object()
    static let mutable clients = 5
    static let mutable roughClients = 0

    static member private AddClient =
        lock _lock (fun () ->
                PiCalculator.VerifyCanAcceptClient
                clients <- clients - 1
                printfn "[Resuorce]: Clients inscrease: %d" clients
            )

    static member private VerifyCanAcceptClient = 
        if clients = 0 then
            roughClients <- roughClients + 1
            if roughClients = 5 then
                printfn "die!!!..."
                Environment.Exit(1);
            else
                invalidOp "max clients count exceeded"
        else
            roughClients <- 0

    static member private RemoveClient =
        lock _lock (fun () ->
                clients <- clients + 1
                printfn "[Resuorce]: Clients decrease: %d" clients
            )

    static member CalcPi = 
        PiCalculator.AddClient
        Thread.Sleep Config.TIMEOUT
        PiCalculator.RemoveClient
        3.14

type State = Open | Lock | HalfOpen

type PiCalculatorCircuitBreaker () = 

    static let _lock = new Object()
    static let threshold = 3
    static let mutable failures = 0
    static let mutable state = State.Open

    static member getPiValue =
        try
            PiCalculatorCircuitBreaker.VerifyNotLocked
            let piAsString = sprintf "%f" PiCalculator.CalcPi
            PiCalculatorCircuitBreaker.HandleSuccess
            OK piAsString
        with
            | :? System.ArgumentException as ex ->
                BAD_REQUEST ex.Message
            | :? System.InvalidOperationException as ex ->
                PiCalculatorCircuitBreaker.HandleFailure
                BAD_REQUEST ex.Message

    static member private VerifyNotLocked =
        lock _lock (fun () ->
                if state = State.Lock then
                    invalidArg "state" "locked"
            )

    static member private HandleFailure =
        printfn "[CircuitBreaker]: Failure request"
        lock _lock (fun () ->
                if state = State.Open then
                    if failures = threshold then
                        PiCalculatorCircuitBreaker.Lock
                    else
                        failures <- failures + 1
                else if state = State.HalfOpen then
                    PiCalculatorCircuitBreaker.Lock
            )

    static member private ChangeState newState =
        state <- newState
        if newState = State.Open then
            printfn "[CircuitBreaker]: Go to Open"
        else if newState = State.Lock then
            printfn "[CircuitBreaker]: Go to Lock"
        else if newState = State.HalfOpen then
            printfn "[CircuitBreaker]: Go to HalfOpen"

    static member private ChangeStateSafe newState =
        lock _lock (fun () ->
                PiCalculatorCircuitBreaker.ChangeState newState
            )

    static member private HandleSuccess =
        printfn "[CircuitBreaker]: Success request"
        lock _lock (fun () ->
                failures <- 0
                if state = State.HalfOpen then
                    PiCalculatorCircuitBreaker.ChangeState State.Open
            )

    static member private Lock =
        PiCalculatorCircuitBreaker.ChangeState State.Lock
        Async.Start (async {
            Thread.Sleep(Config.TIMEOUT)
            PiCalculatorCircuitBreaker.ChangeStateSafe State.HalfOpen
        })

let processRequest request =
    PiCalculatorCircuitBreaker.getPiValue

let app : WebPart =
    choose [
        POST >=> path Config.CALC_PI_ROUTE >=> request processRequest
        NOT_FOUND "Resource not found"
    ]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig app
    0