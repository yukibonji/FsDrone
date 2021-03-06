﻿namespace FsDrone
#nowarn "9"
open System
open Extensions
open FsDrone

//flying substate (minor state)
type FlyingState = 
    | Ok                = 0us
    | LostAlt           = 1us
    | LostAlt_GoDown    = 2us
    | Alt_OutZone       = 3us
    | CombinedYaw       = 4us
    | Brake             = 5us
    | NoVision          = 6us

//controls state: major states
type DroneNavState =
    | Default
    | Init
    | Landed
    | Flying of FlyingState
    | Hovering
    | Test
    | TakingOff
    | GoingToFix
    | Landing
    | Looping

type Velocity = {Vx :float32; Vy :float32; Vz :float32}
type RPY      = {Roll :float32; Pitch :float32; Yaw :float32}
type Magneto  = {Mx :int; My :int; Mz :int}
type Sat      = {SatNum :byte; Cn0 :byte}

type  FlightSummary = 
    {
        Velocity     : Velocity
        RPY          : RPY
        Altitude     : float
        BatteryLevel : float
   }

type  GPS  = 
    { 
        IsGPSPlugged   : bool
        Fix            : bool
        Lat            : float
        Lon            : float
        Alt            : float 
        Heading        : float
        Speed          : float
        NumSats        : int
        SatStrength    : float32
        SatChannels    : Sat list
    }

type Telemetry =
    | NavState      of DroneNavState
    | DroneState    of ArdroneState
    | FlightSummary of FlightSummary
    | Magneto       of Magneto
    | GPS           of GPS

type ConnectionState = Connected of Agent<Command> | Disconnected | Connecting

type TelemetryPortErrors = 
    | MessageSeq | Parse | TooFewBytes | Checksum 
    | UnhandledOption   of NavdataOption 
    | ReceiveError      of Exception
    | KeepAliveError    of Exception

type ConfigPortErrors = ConfigError of Exception

type MonitorMsg =
    | ConnectionError       of Exception
    | CommandPortError      of Exception
    | TelemeteryPortError   of TelemetryPortErrors
    | ConfigPortError       of ConfigPortErrors
    | VideoPortError        of Exception
    | ControlPortError      of Exception
    | ConnectionState       of ConnectionState
    | ScriptError           of string * string
    | HoverLoopError        of Exception

