// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames


let stripInstructionSeparator (points,folds) =
    (points, folds |> List.filter(fun x-> x<>""))
let parseFoldInstructions (points,folds) =
    points, folds
             |> List.map(fun (inst:string) -> inst.Replace("fold along ","") )
             |> List.map(fun inst -> (inst.Split("=").[0], inst.Split("=").[1] |> int))
let printPaper message paper=
    printfn ""
    printfn $"%s{message}"
    printfn "=================="
    printfn ""
    let numCols =  paper |> Set.map(fun (column,row) -> column) |> Set.maxElement
    let numRows =  paper |> Set.map(fun (column,row) -> row) |> Set.maxElement
    for row in [0..numRows] do
        for column in [0..numCols] do
            if paper |> Set.contains (column,row) then printf "#" else printf "."
        printfn ""
   
let rec foldPaper (folds: List<string * int>) (points: Set<int * int>) : Set<int * int> =
    match folds with
    | head::tail ->
        let axis, foldLine = head
        if axis = "y"
        then 
            let beingFolded = points  |> Set.filter(fun (_,row) -> row > foldLine) // anything "below" (with a higher number) the fold line gets folded up.        
            let newPoints = beingFolded |> Set.map(fun(column,row)-> column,foldLine-(row-foldLine) ) // columns don't change, but rows get folded. to their opposite.
            let newMap =
                beingFolded
                |> Set.difference points
                |> Set.union newPoints
            foldPaper tail newMap
        else
            let beingFolded = points  |> Set.filter(fun (column, _) -> column < foldLine) // anything "left" (with a lower number) the fold line gets folded right.
            let newPoints = beingFolded |> Set.map(fun(column,row)-> (foldLine * 2) - column, row ) // rows don't change, but columns get folded to their opposite.
            let newMap =
                beingFolded
                |> Set.difference points
                |> Set.union newPoints
                |> Set.map(fun(column,row)-> column-(foldLine+1), row ) // columns got folded to their opposite, but now they all slide left.
            foldPaper tail newMap
            
    | _ -> points  
            

[<EntryPoint>]
let main argv =

    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data =
        File.ReadLines(fullPath)
        |> List.ofSeq
        
    let separatorIdx =
        data
        |> List.findIndex(fun x -> x = "")
        
    let pts, folds =
        data
        |> List.splitAt separatorIdx
        |> stripInstructionSeparator
        |> parseFoldInstructions

    let points =
            pts
            |> List.map(fun pt -> pt.Split(",") |> Array.map int )
            |> List.map (fun intA -> intA.[0],intA.[1])
            |> Set.ofList
            
    
   
    let foldedPaper = foldPaper folds points
    // at this point we have the code, but it's reversed because it's transparency paper. so we need to switch every X position
    foldedPaper |> printPaper "Code"
    
    foldedPaper
    |> Set.map(fun (column,row) ->
        let midPoint = foldedPaper |> Set.map(fun (c,_) -> c) |> Set.maxElement
        let newColVal =
            if column > midPoint
            then (column-1)-midPoint
            else if column < midPoint then
                     (midPoint*2)-column
            else column
        (newColVal,row ))
    |> printPaper "Reversed"

    
    0 // return an integer exit code
