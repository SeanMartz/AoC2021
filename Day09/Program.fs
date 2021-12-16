// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO
// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

[<EntryPoint>]
let main argv =
    
    let fullPath = Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data = File.ReadLines(fullPath)
               |> List.ofSeq
               |> List.map(fun x -> x
                                    |> Seq.toList
                                    |> Array.ofSeq)
    
    let mutable localizedLowPoints = List.empty
    
    for i, row in data |> Array.ofList |> Array.indexed do
        for j, point in row |> Array.indexed do
            let pointToLeft = if j <> 0 then Some row.[j-1] else None 
            let pointToRight = if j <> row.Length - 1 then Some row.[j+1] else None
            let rowAbove = if i <> 0 then Some data.[i-1] else None
            let rowBelow = if i <> data.Length - 1 then Some data.[i+1] else None
            let pointAbove = match rowAbove with
                             | Some r -> Some r.[j]
                             | _ -> None
            let pointBelow = match rowBelow with
                             | Some r -> Some r.[j]
                             | _ -> None
                             
            if (pointToLeft.IsNone || point < pointToLeft.Value)
               && (pointToRight.IsNone || point < pointToRight.Value)
               && (pointAbove.IsNone ||  point < pointAbove.Value)
               && (pointBelow.IsNone || point < pointBelow.Value)
            then localizedLowPoints <- localizedLowPoints @ [point]

    let answer = localizedLowPoints
                    |> List.map string
                    |> List.map int
                    |> List.map (fun x -> x + 1)
                    |> List.sum
                 
    printfn $"Answer %d{answer}" 
    0 // return an integer exit code