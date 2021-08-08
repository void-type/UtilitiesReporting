module Index

open Elmish
open Fable.Remoting.Client
open Shared

type Model =
    { Todos: Todo list
      Input: string
      MonthlyUsages: MonthlyUsage list }

type Msg =
    | GotTodos of Todo list
    | SetInput of string
    | AddTodo
    | AddedTodo of Todo
    | GotMonthlyUsages of MonthlyUsage list

type MonthlyUsageForChart =
    { Id: int
      Period: string
      Usage: float
      Unit: UsageUnit
      Cost: float }

let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

let init () : Model * Cmd<Msg> =
    let model =
        { Todos = []
          Input = ""
          MonthlyUsages = [] }

    let cmd =
        Cmd.OfAsync.perform todosApi.getMonthlyUsages () GotMonthlyUsages

    model, cmd

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | GotTodos todos -> { model with Todos = todos }, Cmd.none
    | SetInput value -> { model with Input = value }, Cmd.none
    | AddTodo ->
        let todo = Todo.create model.Input

        let cmd =
            Cmd.OfAsync.perform todosApi.addTodo todo AddedTodo

        { model with Input = "" }, cmd
    | AddedTodo todo ->
        { model with
              Todos = model.Todos @ [ todo ] },
        Cmd.none
    | GotMonthlyUsages usages -> { model with MonthlyUsages = usages }, Cmd.none

open Feliz
open Feliz.Bulma
open Feliz.Recharts

let navBrand =
    Bulma.navbarBrand.div [
        Bulma.navbarItem.a [
            prop.href "https://safe-stack.github.io/"
            navbarItem.isActive
            prop.children [
                Html.img [
                    prop.src "/favicon.png"
                    prop.alt "Logo"
                ]
            ]
        ]
    ]

let containerBox (model: Model) (dispatch: Msg -> unit) =
    Bulma.box [
        Bulma.content [
            Html.ol [
                for usage in model.MonthlyUsages do
                    Html.li [
                        prop.text (
                            sprintf
                                "%d-%d %.2f%s $%.2f"
                                usage.Year
                                usage.Month
                                usage.Usage
                                (usage.Unit.ToString())
                                usage.Cost
                        )
                    ]
            ]
        ]
        Bulma.field.div [
            field.isGrouped
            prop.children [
                Bulma.control.p [
                    control.isExpanded
                    prop.children [
                        Bulma.input.text [
                            prop.value model.Input
                            prop.placeholder "What needs to be done?"
                            prop.onChange (SetInput >> dispatch)
                        ]
                    ]
                ]
                Bulma.control.p [
                    Bulma.button.a [
                        color.isPrimary
                        prop.disabled (Todo.isValid model.Input |> not)
                        prop.onClick (fun _ -> dispatch AddTodo)
                        prop.text "Add"
                    ]
                ]
            ]
        ]
    ]

let widget (title: string) (content: ReactElement list) =
    Bulma.box [
        prop.children [
            Bulma.subtitle title
            yield! content
        ]
    ]

let usageGraphWidget (title: string) (usages: MonthlyUsage list) =

    let chartData =
        usages
        |> List.map
            (fun u ->
                { Id = u.Id
                  Period = (sprintf "%d-%d" u.Year u.Month)
                  Usage = float u.Usage
                  Unit = u.Unit
                  Cost = float u.Cost })

    widget
        (sprintf "Monthly %s Usage" title)
        [ Recharts.barChart [
              barChart.layout.horizontal
              barChart.data chartData
              barChart.width 600
              barChart.height 500
              barChart.children [
                  Recharts.cartesianGrid [
                      cartesianGrid.strokeDasharray (4, 4)
                  ]
                  Recharts.xAxis [
                      xAxis.dataKey (fun point -> point.Period)
                      xAxis.width 200
                      xAxis.category
                  ]
                  Recharts.yAxis [ yAxis.number ]
                  Recharts.tooltip []
                  Recharts.bar [
                      bar.legendType.star
                      bar.isAnimationActive true
                      bar.animationEasing.ease
                      bar.dataKey (fun point -> point.Usage)
                      bar.fill "#8884d8"
                  ]
              ]
          ] ]


let view (model: Model) (dispatch: Msg -> unit) =
    Bulma.hero [
        hero.isFullHeight
        color.isPrimary
        prop.style [
            style.backgroundSize "cover"
            style.backgroundImageUrl "https://unsplash.it/1200/900?random"
            style.backgroundPosition "no-repeat center center fixed"
        ]
        prop.children [
            Bulma.heroHead [
                Bulma.navbar [
                    Bulma.container [ navBrand ]
                ]
            ]
            Bulma.heroBody [
                Bulma.container [
                    Bulma.column [
                        column.is6
                        column.isOffset3
                        prop.children [
                            Bulma.title [
                                text.hasTextCentered
                                prop.text "UtilitiesReporting"
                            ]
                            containerBox model dispatch
                        ]
                    ]
                ]
            ]
            usageGraphWidget "Electricity" model.MonthlyUsages
        ]
    ]
