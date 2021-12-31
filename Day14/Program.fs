// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

// Define a function to construct a message to print
let stripInstructionSeparator (polymerTemplate, insertionRules) =
    (polymerTemplate, insertionRules |> List.filter (fun x -> x <> ""))

let splitFormulaIntoMap (polymerTemplate, insertionRules) =
    polymerTemplate,
    insertionRules
    |> List.map (fun (formula: string) -> (formula.Split(" -> ").[0], formula.Split(" -> ").[1]))
    |> Map.ofList

let iterateFormula insertionRules polymerTemplate =
    let newPolymerTemplate =
        polymerTemplate
        |> Seq.pairwise
        |> List.ofSeq
        |> List.map(fun (a,b) -> string a, string b)
        |> List.map
            (fun (a,b) ->
                match insertionRules |> Map.tryFind ( a + b) with
                | Some ltr ->  a + ltr
                | None ->  a +  b)
        |> String.concat ""
    newPolymerTemplate + string (polymerTemplate |>Seq.last)
    
[<EntryPoint>]
let main argv =

    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data = File.ReadLines(fullPath) |> List.ofSeq

    let separatorIdx = data |> List.findIndex (fun x -> x = "")

    let templates, insertionRules =
        data
        |> List.splitAt separatorIdx
        |> stripInstructionSeparator
        |> splitFormulaIntoMap

    let polymerTemplate = templates |> List.head

    let gen10 =
               polymerTemplate
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
               |> iterateFormula insertionRules
            
    let counts =
        gen10
        |> Seq.countBy id
        |> Seq.map( fun (ltr, count) -> count)

    
    let answer = (counts |> Seq.max) - (counts |> Seq.min)

    printfn $"answer: %d{answer}"
    0 // return an integer exit code
