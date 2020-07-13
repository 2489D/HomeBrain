// Learn more about F# at http://fsharp.org

open System
open System.Threading
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

[<EntryPoint>]
let main argv =
    let app =
        choose
            [   GET >=> choose
                    [ path "/hello" >=> OK "Hello GET" ]
                POST >=> choose
                    [ path "/hello" >=> OK "Hello POST" ] ]
    startWebServer defaultConfig app
    0 // return an integer exit code
