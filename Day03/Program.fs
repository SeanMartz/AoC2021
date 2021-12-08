// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

// Define a function to construct a message to print
let rec getColumnAverages (data: list<string>) (columnAverages: list<float>) : list<float> =
    if data.Head.Length > 0 then
        let average =
            data
            |> List.map (fun binary -> binary.[0].ToString())
            |> List.map Int32.Parse
            |> List.map float
            |> List.average

        let stripFirstColumn =
            data |> List.map (fun b -> b.Substring(1))

        getColumnAverages stripFirstColumn (columnAverages @ [ average ])
    else
        columnAverages


let calculateMostCommonBit data : string =
    getColumnAverages data []
    |> List.map (fun avg -> if avg >= 0.5 then 1.0 else 0.0)
    |> List.map Convert.ToInt32
    |> List.map string
    |> Seq.ofList
    |> String.concat ""


let calculateLeastCommonBit data : string =
    getColumnAverages data []
    |> List.map (fun avg -> if avg >= 0.5 then 0.0 else 1.0)
    |> List.map Convert.ToInt32
    |> List.map string
    |> Seq.ofList
    |> String.concat ""


let rec filterDataForMostCommonBitInColumn (data: list<string>) (commonBit: string) (position: int) : string =
    if data.Length <> 1 then
        let filteredData =
            data
            |> List.filter (fun binary -> binary.[position] = commonBit.[position])

        let mostCommonBit = calculateMostCommonBit filteredData
        filterDataForMostCommonBitInColumn filteredData mostCommonBit (position + 1)
    else
        data.Head

let rec filterDataForLeastCommonBitInColumn (data: list<string>) (commonBit: string) (position: int) : string =
    if data.Length <> 1 then
        let filteredData =
            data
            |> List.filter (fun binary -> binary.[position] = commonBit.[position])

        let leastCommonBit = calculateLeastCommonBit filteredData
        filterDataForLeastCommonBitInColumn filteredData leastCommonBit (position + 1)
    else
        data.Head

[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data = File.ReadLines(fullPath) |> List.ofSeq

    let gamma = calculateMostCommonBit data
    let epsilon = calculateLeastCommonBit data
    let gammaNumber = Convert.ToInt32(gamma, 2)
    let epsilonNumber = Convert.ToInt32(epsilon, 2)
    let powerConsumption = gammaNumber * epsilonNumber

    printfn $"Power Consumption:  %d{powerConsumption}"

    let oxygen = filterDataForMostCommonBitInColumn data gamma 0
    let co2 = filterDataForLeastCommonBitInColumn data epsilon 0
    let oxygenNumber = Convert.ToInt32(oxygen, 2)
    let co2Number = Convert.ToInt32(co2, 2)
    let lifeSupportRating = oxygenNumber * co2Number

    printfn $"LifeSupport Rating: %d{lifeSupportRating}"
    0 // return an integer exit code
