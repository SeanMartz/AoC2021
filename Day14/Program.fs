// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO


type Rules = Map<char * char, char>

type Polymer = { PairCounts: Map<char * char, bigint> }
// Define a function to construct a message to print
let stripInstructionSeparator (polymerTemplate, insertionRules) =
    (polymerTemplate
     |> List.head
     , insertionRules
       |> List.filter (fun x -> x <> ""))

let splitFormulaIntoMap (polymerTemplate, insertionRules) =
    polymerTemplate,
    insertionRules
    |> List.map (fun (formula: string) -> (formula.Split(" -> ").[0], formula.Split(" -> ").[1]))
    |> Map.ofList
//
//let iterateFormula insertionRules polymerTemplate =
//    let newPolymerTemplate =
//        polymerTemplate
//        |> Seq.pairwise
//        |> List.ofSeq
//        |> List.map(fun (a,b) -> string a, string b)
//        |> List.map
//            (fun (a,b) ->
//                match insertionRules |> Map.tryFind ( a + b) with
//                | Some ltr ->  a + ltr
//                | None ->  a +  b)
//        |> String.concat ""
//    newPolymerTemplate + string (polymerTemplate |>Seq.last)
//    
[<EntryPoint>]
let main argv =

    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "example.txt")

    let data = File.ReadLines(fullPath) |> List.ofSeq

    let separatorIdx = data |> List.findIndex (fun x -> x = "")

    let polymerTemplate, insertionRules =
        data
        |> List.splitAt separatorIdx
        |> stripInstructionSeparator
        |> splitFormulaIntoMap
    
    let pairsCount = Map.empty
    
    let answer = doFor 40 
//    let polymerTemplatePairs =
//        polymerTemplate
//        |> Seq.pairwise
//        |> Seq.map( fun (a,b) ->
//            match insertionRules.TryFind (a,b) with
//            | Some mid ->
//                pairsCount
//                |> Map.change pair (fun x ->
//                                        match x with
//                                        | Some value -> Some(value + mid)
//                                        | None -> Some pair)
//                string a + mid
//            | None -> pair
//            )


   
//    let counts =
//        gen10
//        |> Seq.countBy id
//        |> Seq.map( fun (ltr, count) -> count)
//
//    
//    let answer = (counts |> Seq.max) - (counts |> Seq.min)

//    printfn $"answer: %d{answer}"
    0 // return an integer exit code
