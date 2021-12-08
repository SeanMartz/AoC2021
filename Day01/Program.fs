// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

let rec CountIncreasesBetweenLines (data: int list) (count: int) : int =
    match data with
    | head :: tail ->
        if not tail.IsEmpty && head < tail.Head then
            CountIncreasesBetweenLines tail (count + 1)
        else
            CountIncreasesBetweenLines tail count
    | [] -> count


let sumTop3 (data: int list) : int =
    data.Head + data.Tail.Head + data.Tail.Tail.Head
    
let has3ValuesLeft (data: int list) : bool =
    not data.Tail.IsEmpty
    && not data.Tail.Tail.IsEmpty
    && not data.Tail.Tail.Tail.IsEmpty

let rec CountIncreasesBetweenSlidingWindow (data : int list ) (count) : int =
    match data with
    | head :: tail ->
        if has3ValuesLeft data && (sumTop3 tail) > (sumTop3 data) then
            CountIncreasesBetweenSlidingWindow tail (count + 1)
        else
            CountIncreasesBetweenSlidingWindow tail count
    | [] -> count

// Define a function to construct a message to print
[<EntryPoint>]
let main argv =
    let fullPath = Path.Combine(__SOURCE_DIRECTORY__, "test.txt")

    let data = File.ReadLines(fullPath)
    let dataInt = data |> List.ofSeq |> List.map int

    let increases = CountIncreasesBetweenLines dataInt 0

    printfn $"%d{increases} Increases between lines"
    
    let windowIncreases = CountIncreasesBetweenSlidingWindow dataInt 0
    printfn $"%d{windowIncreases} Increases in sliding window"
    0 // return an integer exit code
