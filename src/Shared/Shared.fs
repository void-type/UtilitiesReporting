namespace Shared

type MonthlyUsage =
    { Id: int
      Year: int
      Month: int
      Usage: decimal
      Cost: decimal }


module UsageUnits =
    let usageTypes =
        [ "electric", "KW"
          "gas", "BTU"
          "water", "KGal" ]
        |> Map.ofList

    let getFromUtilityType usageType =
        let found = usageTypes.TryFind usageType

        match found with
        | Some x -> x
        | None -> ""

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IAppApi =
    { GetMonthlyUsages: string -> Async<Result<MonthlyUsage list, string>> }
