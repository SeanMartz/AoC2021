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
    
let foldPaper (folds: List<string * int>) (points: Set<int * int>) : Set<int * int> =
    let axis, foldLine = folds.Head
    let beingFolded =
                points
                |> Set.filter(fun (x,y) -> if axis = "y" then y > foldLine else x < foldLine)
                
    let newPoints =
                beingFolded
                |> Set.map(fun(x,y)-> if axis = "y" then x,foldLine-(y-foldLine) else foldLine+(foldLine-x),y)
    let newMap =
        beingFolded
        |> Set.difference points
        |> Set.union newPoints
    
    if axis = "x"
    then newMap |> Set.map(fun (x,y)-> x-(foldLine+1),y)
    else newMap
    

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
            
    let firstFold = foldPaper folds points
    let secondFold = foldPaper folds.Tail firstFold
      
                
    
    printfn $"firstFold: %d{firstFold.Count}"
    printfn $"secondFold: %d{secondFold.Count}"
    0 // return an integer exit code
