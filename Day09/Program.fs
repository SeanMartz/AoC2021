// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

let getNeighborValues (x, y) floor =
        [ (-1, 0); (1, 0); (0, -1); (0, 1) ]
        |> List.map (fun (neighborX, neighborY) -> (x + neighborX, y + neighborY))
        |> List.choose (fun location -> floor |> Map.tryFind location)
    
type FloorMap = Map<int *int, int>

[<EntryPoint>]
let main argv =
    
    let fullPath = Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data = File.ReadLines(fullPath)
               |> Array.ofSeq
               |> Array.map(fun x -> x
                                    |> Seq.toList
                                    |> Array.ofSeq
                                    |> Array.map string// string -> int conversion necessary otherwise i'll get the characters encoding number
                                    |> Array.map int)
    
    let floorMap =
        [for i, row in data |> Array.indexed do
            for j, point in row |> Array.indexed -> (i, j), point ]
        |> Map.ofList
        
    let lowPoints =
        floorMap
        |> Map.toList
        |> List.map(fun (location, value) -> (location, value), floorMap |> getNeighborValues location)
        |> List.filter(fun (locAndVal, neighborValues) -> snd locAndVal < (neighborValues |> List.min ))
        |> List.map(fun (locAndVal, _) -> (fst locAndVal, snd locAndVal))
        
    let answer = lowPoints
                |> List.map(fun (_, value) -> value + 1)
                |> List.sum
                 
    printfn $"Answer %d{answer}" 
    0 // return an integer exit code