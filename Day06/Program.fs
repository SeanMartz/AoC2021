// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO


let rec dayPass (currentDay: int) (maxDays: int) (fish: list<int>) : List<int> =
    match currentDay.Equals maxDays with
    | true -> fish
    | _ ->
        dayPass (currentDay + 1) maxDays fish
                                          |> List.map (fun f -> f - 1)
                                          |> List.map (fun f -> if f = -1 then [ 8 ] @ [ 6 ] else [ f ])
                                          |> List.concat

[<EntryPoint>]
let main argv =

    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let puzzleInput = File.ReadLines(fullPath) |> List.ofSeq

    let lanternFish =
        puzzleInput.Head.Split(",")
        |> List.ofArray
        |> List.map int

    let newLanternFish = dayPass 0 80 lanternFish

    printfn $"Fish after 80 days %d{newLanternFish.Length}"
    0 // return an integer exit code
