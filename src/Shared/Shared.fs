namespace Shared

open System

type Todo = { Id: Guid; Description: string }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          Description = description }

type UsageUnit =
    | KW
    | BTU
    | KGal

type MonthlyUsage =
    { Id: int
      Year: int
      Month: int
      Usage: decimal
      Unit: UsageUnit
      Cost: decimal }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ITodosApi =
    { getTodos: unit -> Async<Todo list>
      addTodo: Todo -> Async<Todo>
      getMonthlyUsages: unit -> Async<MonthlyUsage list>}
