module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared

type Storage() =
    let todos = ResizeArray<_>()
    let monthlyUsages = ResizeArray<_>()

    member __.GetTodos() = List.ofSeq todos

    member __.AddTodo(todo: Todo) =
        if Todo.isValid todo.Description then
            todos.Add todo
            Ok()
        else
            Error "Invalid todo"

    member __.GetMonthlyUsages() = List.ofSeq monthlyUsages

    member __.AddMonthlyUsage(usage: MonthlyUsage) =
        monthlyUsages.Add usage
        Ok()

let storage = Storage()

storage.AddTodo(Todo.create "Create new SAFE project")
|> ignore

storage.AddTodo(Todo.create "Write your app")
|> ignore

storage.AddTodo(Todo.create "Ship it !!!")
|> ignore

storage.AddMonthlyUsage(
    { Id = 1
      Year = 2021
      Month = 2
      Usage = 300m
      Unit = KW
      Cost = 150m }
)
|> ignore

storage.AddMonthlyUsage(
    { Id = 2
      Year = 2021
      Month = 3
      Usage = 350m
      Unit = KW
      Cost = 175m }
)
|> ignore

storage.AddMonthlyUsage(
    { Id = 3
      Year = 2021
      Month = 4
      Usage = 400m
      Unit = KW
      Cost = 300m }
)
|> ignore

storage.AddMonthlyUsage(
    { Id = 4
      Year = 2021
      Month = 5
      Usage = 300m
      Unit = KW
      Cost = 150m }
)
|> ignore

let todosApi =
    { getTodos = fun () -> async { return storage.GetTodos() }
      addTodo =
          fun todo ->
              async {
                  match storage.AddTodo todo with
                  | Ok () -> return todo
                  | Error e -> return failwith e
              }
      getMonthlyUsages = fun () -> async { return storage.GetMonthlyUsages() } }

let webApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue todosApi
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
