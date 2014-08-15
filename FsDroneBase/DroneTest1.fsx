﻿#load "SetUpEnv.fsx"

open FsDrone
open System
open System.Threading

let cntlr = new DroneController()
let stop() = (cntlr :> IDisposable).Dispose()
(*
stop()
*)

let sub1 = cntlr.Monitor.Subscribe(fun msg -> printfn "%A" msg)
let connectionCts = new CancellationTokenSource()
let sub2 = ref (None:IDisposable option)


Async.Start (
    async {
    let! ot = cntlr.ConnectAsync connectionCts 
    let s = ot.Subscribe(fun msg -> printfn "%A" msg) 
    sub2 := Some s},
    connectionCts.Token)

cntlr.Emergency()

let isLanded = function Landed      -> true | _ -> false
let isFlying = function Flying _    -> true | _ -> false

cntlr.Run (WhenRepeat (isLanded, Takeoff, isFlying))
cntlr.Run(FsDrone.CommandScript.Single Takeoff)