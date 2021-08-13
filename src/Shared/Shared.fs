namespace Shared

type MonthlyUsage =
    { Id: int
      Year: int
      Month: int
      Usage: decimal
      Cost: decimal }

    member this.DateLabel = sprintf "%d-%d" this.Year this.Month

type Choice = { Name: string; Value: string }

type UtilityType =
    | Electric
    | Gas
    | Water

type Utility =
    { Type: UtilityType
      Label: string
      ChoiceValue: string
      UnitLabel: string }

module Utility =
    let all =
        [ { Type = Electric
            Label = "Electric"
            ChoiceValue = "electric"
            UnitLabel = "kWh" }
          { Type = Gas
            Label = "Gas"
            ChoiceValue = "gas"
            UnitLabel = "CCF" }
          { Type = Water
            Label = "Water"
            ChoiceValue = "water"
            UnitLabel = "kGal" } ]

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IAppApi =
    { GetMonthlyUsages: UtilityType -> Async<Result<MonthlyUsage list, string>> }
