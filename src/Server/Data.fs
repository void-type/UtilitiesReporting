module Data

open Shared

type Storage() as this =
    let electric = ResizeArray<_>()

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
              Usage = 400m
              Cost = 300m }
        )
        |> ignore

    do
        this.AddElectricUsage(
            { Id = 4
              Year = 2021
              Month = 5
              Usage = 300m
              Cost = 150m }
        )
        |> ignore

    member __.GetElectricUsages() = Ok (List.ofSeq electric)

    member __.AddElectricUsage(usage: MonthlyUsage) =
        electric.Add usage
        Ok()
