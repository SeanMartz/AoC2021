// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

let reduceKeys (generationMap: Map<int, int64>) : Map<int, int64> =
    generationMap
    |> Map.toSeq
    |> Seq.map (fun tpl -> (fst tpl - 1, snd tpl))
    |> Map.ofSeq

let populate (generationMap: Map<int, int64>) : Map<int, int64> =
    let newFishOpt = generationMap |> Map.tryFind -1
    
    let newFish =
        match newFishOpt with
        | Some v -> v
        | None -> int64 0

    generationMap
    |> Map.change 8 (fun x ->
            match x with
            | Some value -> Some(value + newFish)
            | None -> Some newFish)
    |> Map.change 6 (fun x ->
            match x with
            | Some value -> Some(value + newFish)
            | None -> Some newFish)
    |> Map.remove -1

let rec doFor (times:int) (fishMap: Map<int,int64>) :Map<int,int64> = 
    match times.Equals 0 with
    | true -> fishMap
    | _ -> doFor (times-1) fishMap
                            |> reduceKeys
                            |> populate

[<EntryPoint>]
let main argv =

    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let puzzleInput = File.ReadLines(fullPath) |> List.ofSeq
        
    let initialLanternFish =
        (puzzleInput |> Seq.head).Split(",")
        |> Seq.ofArray
        |> Seq.map int
    
    let fishMap = initialLanternFish |> Seq.countBy id |> Seq.map(fun v -> (fst v, int64 (snd v)) ) |> Map.ofSeq
    
    let eightyGen = doFor 80 fishMap
    let twoFiftySixGen = doFor 256 fishMap
    
    let answerEighty = eightyGen |> Map.toSeq |> Seq.map (fun tpl -> snd tpl) |> Seq.sum
    printfn $"Fish after 80 days %d{answerEighty}"

    let answerTwoFiftySix = twoFiftySixGen |> Map.toSeq |> Seq.map (fun tpl -> snd tpl) |> Seq.sum
    printfn $"Fish after 256 days %d{answerTwoFiftySix}"

    0 // return an integer exit code
