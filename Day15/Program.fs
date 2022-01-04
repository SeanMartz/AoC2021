// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System

type Node =
    { position: int*int
      riskLevel: int }
// Define a function to construct a message to print
let getNeighborValues (x, y) grid =
        [ (-1, 0); (1, 0); (0, -1); (0, 1) ]
        |> List.map (fun (neighborX, neighborY) -> (x + neighborX, y + neighborY))
        |> List.choose (fun location -> grid |> Map.tryFind location)
let from whom =
    sprintf "from %s" whom

[<EntryPoint>]
let main argv =
    
    let fullPath =
        IO.Path.Combine(__SOURCE_DIRECTORY__, "example.txt")

    let data =
        IO.File.ReadLines(fullPath)
        |> Array.ofSeq
        |> Array.map(fun row -> row  |> Array.ofSeq |> Array.map string)
        
    let grid =
        [for i, row in data |> Array.indexed do
            for j, point in row |> Array.indexed -> (i, j), point ]
        |> Map.ofList

        
    let message = from "F#" // Call the function
    printfn "Hello world %s" message
    0 // return an integer exit code