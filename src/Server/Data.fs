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

    member __.GetElectricUsages() = Ok (List.ofSeq electric)

    member __.AddElectricUsage(usage: MonthlyUsage) =
        electric.Add usage
        Ok()

    member __.GetGasUsages() = Ok (List.ofSeq gas)

    member __.AddGasUsage(usage: MonthlyUsage) =
        gas.Add usage
        Ok()

    member __.GetWaterUsages() = Ok (List.ofSeq water)

    member __.AddWaterUsage(usage: MonthlyUsage) =
        water.Add usage
        Ok()
