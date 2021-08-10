module Index

open Elmish
open Fable.Remoting.Client
open Shared
open System

type Model =
    { MonthlyUsages: MonthlyUsage list
      UtilityType: string
      ErrorMessage: string }

type Msg = GotMonthlyUsages of Result<MonthlyUsage list, string>

type MonthlyUsageForChart =
    { Id: int
      Period: string
      Usage: float
      Cost: float }

let api =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IAppApi>

let init () : Model * Cmd<Msg> =
    let model =
        { MonthlyUsages = []
          UtilityType = "electric"
          ErrorMessage = "" }

    let cmd =
        Cmd.OfAsync.perform api.GetMonthlyUsages model.UtilityType GotMonthlyUsages

    model, cmd

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | GotMonthlyUsages usagesResult ->
        match usagesResult with
        | Ok usages ->
            { model with
                  MonthlyUsages = usages
                  ErrorMessage = "" },
            Cmd.none
        | Error e -> { model with ErrorMessage = e }, Cmd.none

open Feliz
open Feliz.Bulma
open Feliz.Recharts

let usageGraphWidget (usages: MonthlyUsage list) (utilityType: string) =

    let chartData =
        usages
        |> List.map
            (fun u ->
                { Id = u.Id
                  Period = (sprintf "%d-%d" u.Year u.Month)
                  Usage = float u.Usage
                  Cost = float u.Cost })

    let chart =
        Recharts.barChart [
            barChart.data chartData
            barChart.layout.horizontal
            barChart.children [
                Recharts.cartesianGrid [
                    cartesianGrid.strokeDasharray (4, 4)
                ]
                Recharts.xAxis [
                    xAxis.dataKey (fun point -> point.Period)
                    xAxis.width 200
                    xAxis.category
                ]
                Recharts.yAxis [
                    yAxis.number
                    UsageUnits.getFromUtilityType utilityType
                    |> yAxis.unit
                ]
                Recharts.tooltip []
                Recharts.bar [
                    bar.legendType.star
                    bar.isAnimationActive true
                    bar.animationEasing.ease
                    bar.dataKey (fun point -> point.Usage)
                    bar.fill "#58a6ff"
                ]
            ]
        ]

    Bulma.box [
        prop.children [
            Bulma.subtitle (sprintf "Monthly %s usage" utilityType)
            Recharts.responsiveContainer [
                responsiveContainer.width (length.percent 100)
                responsiveContainer.height 400
                responsiveContainer.chart chart
            ]
        ]
    ]

let view (model: Model) (dispatch: Msg -> unit) =
    Bulma.container [
        Bulma.title [
            title.is1
            color.isPrimary
            text.hasTextCentered
            prop.text "Utilities Reporting"
        ]
        if not (String.IsNullOrWhiteSpace(model.ErrorMessage)) then
            Bulma.notification [
                color.isDanger
                prop.text model.ErrorMessage
            ]
        usageGraphWidget model.MonthlyUsages model.UtilityType
    ]
