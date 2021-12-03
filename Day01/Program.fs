// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

let rec CountIncreasesBetweenLines (data: int list) (count: int) : int =
    match data with
    | head :: tail ->
        if tail.Length > 0 && head < tail.Head then
            CountIncreasesBetweenLines tail (count + 1)
        else
            CountIncreasesBetweenLines tail count
    | [] -> count


// Define a function to construct a message to print
[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "test.txt")

    let data = File.ReadLines(fullPath)
    let dataInt = data |> List.ofSeq |> List.map int

    let increases = CountIncreasesBetweenLines dataInt 0

    printfn $"%d{increases} Increases between lines"
    0 // return an integer exit code
