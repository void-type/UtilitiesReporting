module Data

open Shared

type Storage() as this =
    let electric = ResizeArray<_>()
    let gas = ResizeArray<_>()
    let water = ResizeArray<_>()

    do
        this.AddElectricUsage(
            { Id = 1
              Year = 2021
              Month = 2
              Usage = 300m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddElectricUsage(
            { Id = 2
              Year = 2021
              Month = 3
              Usage = 350m
              Cost = 175m }
        )
        |> ignore

    do
        this.AddElectricUsage(
            { Id = 3
              Year = 2021
              Month = 4
              Usage = 450m
              Cost = 300m }
        )
        |> ignore

    do
        this.AddElectricUsage(
            { Id = 4
              Year = 2021
              Month = 5
              Usage = 600m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddGasUsage(
            { Id = 1
              Year = 2021
              Month = 2
              Usage = 110m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddGasUsage(
            { Id = 2
              Year = 2021
              Month = 3
              Usage = 90m
              Cost = 175m }
        )
        |> ignore

    do
        this.AddGasUsage(
            { Id = 3
              Year = 2021
              Month = 4
              Usage = 60m
              Cost = 300m }
        )
        |> ignore

    do
        this.AddGasUsage(
            { Id = 4
              Year = 2021
              Month = 5
              Usage = 20m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 1
              Year = 2020
              Month = 2
              Usage = 3m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 2
              Year = 2020
              Month = 3
              Usage = 3m
              Cost = 175m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 3
              Year = 2020
              Month = 4
              Usage = 5m
              Cost = 300m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 4
              Year = 2020
              Month = 5
              Usage = 10m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 1
              Year = 2020
              Month = 6
              Usage = 3m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 2
              Year = 2020
              Month = 7
              Usage = 3m
              Cost = 175m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 3
              Year = 2020
              Month = 8
              Usage = 5m
              Cost = 300m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 4
              Year = 2020
              Month = 9
              Usage = 10m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 1
              Year = 2020
              Month = 10
              Usage = 3m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 2
              Year = 2020
              Month = 11
              Usage = 3m
              Cost = 175m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 3
              Year = 2020
              Month = 12
              Usage = 5m
              Cost = 300m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 4
              Year = 2021
              Month = 1
              Usage = 10m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 1
              Year = 2021
              Month = 2
              Usage = 3m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 2
              Year = 2021
              Month = 3
              Usage = 3m
              Cost = 175m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 3
              Year = 2021
              Month = 4
              Usage = 5m
              Cost = 300m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 4
              Year = 2021
              Month = 5
              Usage = 10m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 1
              Year = 2021
              Month = 6
              Usage = 3m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 2
              Year = 2021
              Month = 7
              Usage = 3m
              Cost = 175m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 3
              Year = 2021
              Month = 8
              Usage = 5m
              Cost = 300m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 4
              Year = 2021
              Month = 9
              Usage = 10m
              Cost = 150m }
        )
        |> ignore

    do
        this.AddWaterUsage(
            { Id = 1
              Year = 2021
              Month = 10
              Usage = 3m
              Cost = 150m }
        )
        |> ignore

    member __.GetElectricUsages() =
        try
            Ok(List.ofSeq electric)
        with
        | _ -> Error "Error contacting database."

    member __.AddElectricUsage(usage: MonthlyUsage) =
        try
            electric.Add usage
            Ok()
        with
        | _ -> Error "Error contacting database."

    member __.GetGasUsages() =
        try
            Ok(List.ofSeq gas)
        with
        | _ -> Error "Error contacting database."

    member __.AddGasUsage(usage: MonthlyUsage) =
        try
            gas.Add usage
            Ok()
        with
        | _ -> Error "Error contacting database."

    member __.GetWaterUsages() =
        try
            Ok(List.ofSeq water)
        with
        | _ -> Error "Error contacting database."

    member __.AddWaterUsage(usage: MonthlyUsage) =
        try
            water.Add usage
            Ok()
        with
        | _ -> Error "Error contacting database."
