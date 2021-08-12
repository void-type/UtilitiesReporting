module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared
open Data

let data = Storage()

let api =
    { GetMonthlyUsages =
          fun utility ->
              async {
                  match utility with
                  | Electric -> return data.GetElectricUsages()
                  | Gas -> return data.GetGasUsages()
                  | Water -> return data.GetWaterUsages()
                  | _ -> return Error "Invalid utility selection."
              } }

let webApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue api
    |> Remoting.buildHttpHandler

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
    }

run app
