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


let calculateGamma data : string =
    getColumnAverages data []
    |> List.map (fun avg -> if avg >= 0.5 then 1.0 else 0.0)
    |> List.map Convert.ToInt32
    |> List.map string
    |> Seq.ofList
    |> String.concat ""


let calculateEpsilon data : string =
    getColumnAverages data []
    |> List.map (fun avg -> if avg >= 0.5 then 0.0 else 1.0)
    |> List.map Convert.ToInt32
    |> List.map string
    |> Seq.ofList
    |> String.concat ""


[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data = File.ReadLines(fullPath) |> List.ofSeq

    let gamma = calculateGamma data
    let epsilon = calculateEpsilon data
    let gammaNumber = Convert.ToInt32(gamma, 2)
    let epsilonNumber = Convert.ToInt32(epsilon, 2)
    let powerConsumption = gammaNumber * epsilonNumber

    printfn $"Gamma Binary:  %s{gamma}"
    printfn $"Gamma Base10: %d{gammaNumber}"
    printfn $"Epsilon Binary:  %s{epsilon}"
    printfn $"Epsilon Base10:  %d{epsilonNumber}"
    printfn $"Power Consumption:  %d{powerConsumption}"
    0 // return an integer exit code
