namespace Shared

open Microsoft.FSharp.Reflection

type MonthlyUsage =
    { Id: int
      Year: int
      Month: int
      Usage: decimal
      Cost: decimal }

type UtilityType =
    | Electric
    | Gas
    | Water

module UtilityType =
    let getUnit utility =
        match utility with
        | Electric -> "kWh"
        | Gas -> "CCF"
        | Water -> "kGal"

    let getTypes =
        FSharpType.GetUnionCases(typeof<UtilityType>)
        |> Seq.map (fun x -> x.Name)

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IAppApi =
    { GetMonthlyUsages: UtilityType -> Async<Result<MonthlyUsage list, string>> }
