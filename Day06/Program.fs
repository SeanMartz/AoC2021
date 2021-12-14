// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO


let rec dayPass (currentDay: int) (maxDays: int) (fish: seq<int>) : seq<int> =
    match currentDay.Equals maxDays with
    | true -> fish
    | _ ->
        dayPass (currentDay + 1) maxDays fish
                                          |> Seq.map (fun f -> f - 1)
                                          |> Seq.map (fun f -> if f = -1 then [ 8 ] @ [ 6 ] else [ f ])
                                          |> Seq.concat

[<EntryPoint>]
let main argv =

    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let puzzleInput = File.ReadLines(fullPath) |> List.ofSeq

    let lanternFish = (puzzleInput |> Seq.head).Split(",")
                                                |> Seq.ofArray
                                                |> Seq.map int

    let newLanternFish80 = dayPass 0 80 lanternFish

    printfn $"Fish after 80 days %d{newLanternFish80 |> Seq.length}"
    0 // return an integer exit code
