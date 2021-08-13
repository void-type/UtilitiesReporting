module Index

open Elmish
open Fable.Remoting.Client
open Shared
open System

type Model =
    { MonthlyUsages: MonthlyUsage list
      SelectedUtility: Utility
      ErrorMessage: string }

type Msg =
    | GetMonthlyUsages
    | GotMonthlyUsages of Result<MonthlyUsage list, string>
    | UtilityTypeChanged of string
    | ErrorMsg of string

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
          SelectedUtility = Utility.all.[0]
          ErrorMessage = "" }

    let cmd = Cmd.ofMsg GetMonthlyUsages

    model, cmd

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | GetMonthlyUsages -> model, Cmd.OfAsync.perform api.GetMonthlyUsages model.SelectedUtility.Type GotMonthlyUsages

    | GotMonthlyUsages usagesResult ->
        match usagesResult with
        | Ok usages ->
            { model with
                  MonthlyUsages = usages |> List.sortBy (fun x -> x.DateLabel)
                  ErrorMessage = "" },
            Cmd.none
        | Error e -> model, Cmd.ofMsg (ErrorMsg e)

    | ErrorMsg e -> { model with ErrorMessage = e }, Cmd.none

    | UtilityTypeChanged choice ->
        let utility =
            Utility.all
            |> List.tryFind (fun x -> x.ChoiceValue = choice)

        match utility with
        | Some u -> { model with SelectedUtility = u }, Cmd.ofMsg GetMonthlyUsages
        | None -> model, Cmd.ofMsg (ErrorMsg(sprintf "Invalid utility selection (%s)." choice))

open Feliz
open Feliz.Bulma
open Feliz.Recharts

let usageGraphWidget (usages: MonthlyUsage list) (utility: Utility) =

    let chartData =
        usages
        |> List.map
            (fun u ->
                { Id = u.Id
                  Period = u.DateLabel
                  Usage = float u.Usage
                  Cost = float u.Cost })

    let chart =
        Recharts.lineChart [
            lineChart.data chartData
            lineChart.layout.horizontal
            lineChart.children [
                Recharts.cartesianGrid [
                    cartesianGrid.strokeDasharray (4, 4)
                ]
                Recharts.xAxis [
                    xAxis.dataKey (fun point -> point.Period)
                    xAxis.width 200
                    xAxis.category
                ]
                Recharts.tooltip []
                Recharts.yAxis [
                    yAxis.yAxisId "left"
                    yAxis.unit utility.UnitLabel
                    yAxis.name "Usage"
                ]
                Recharts.line [
                    line.yAxisId "left"
                    line.dataKey (fun point -> point.Usage)
                    line.stroke "#58a6ff"
                    line.isAnimationActive true
                    line.animationEasing.ease
                ]
                Recharts.yAxis [
                    yAxis.yAxisId "right"
                    yAxis.orientation.right
                    yAxis.unit "usd"
                    yAxis.name "Cost"
                ]
                Recharts.line [
                    line.yAxisId "right"
                    line.dataKey (fun point -> point.Cost)
                    line.stroke "#82ca9d"
                    line.isAnimationActive true
                    line.animationEasing.ease
                ]
            ]
        ]

    Bulma.box [
        prop.children [
            Bulma.subtitle (sprintf "Monthly %s usage" utility.Label)
            Recharts.responsiveContainer [
                responsiveContainer.width (length.percent 100)
                responsiveContainer.height 500
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

        Bulma.field.div [
            Bulma.label "Utility type"
            Bulma.control.div [
                prop.children [
                    Bulma.select [
                        prop.onChange (UtilityTypeChanged >> dispatch)
                        prop.children (
                            Utility.all
                            |> List.map
                                (fun u ->
                                    Html.option [
                                        prop.value u.ChoiceValue
                                        prop.text u.Label
                                    ])
                        )
                    ]
                ]
            ]
        ]

        usageGraphWidget model.MonthlyUsages model.SelectedUtility
    ]
