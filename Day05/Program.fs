// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

// Define a function to construct a message to print
type ventPoint = { start: int * int; finish: int * int }

let rec generatePointsBetween points pointList : list<(int * int)> =
    match points with
    | head :: tail ->
        let xs = if fst head.start < fst head.finish then [ (fst head.start) .. (fst head.finish) ] else [ (fst head.finish) .. (fst head.start) ]
        let ys = if snd head.start < snd head.finish then [ (snd head.start) .. (snd head.finish) ] else [ (snd head.finish) .. (snd head.start) ]
        let isStraight = xs.Length = 1 || ys.Length = 1 
        let newPoints =
            if isStraight then
                if xs.Length = 1 then
                    ((Array.create ys.Length xs.Head |> List.ofArray), ys)
                    ||> List.map2 (fun x y -> (x, y))
                else
                    ((Array.create xs.Length ys.Head |> List.ofArray), xs)
                    ||> List.map2 (fun y x -> (x, y))
            else
                let xsOrdered = if fst head.start > fst head.finish then xs |> List.sortDescending else xs |> List.sort
                let ysOrdered = if snd head.start > snd head.finish then ys |> List.sortDescending  else ys |> List.sort 
                (xsOrdered, ysOrdered) ||> List.map2(fun x y -> (x, y))
                
        generatePointsBetween tail (pointList @ newPoints)
    | [] -> pointList

let xOrYMatch vp =
    let (x1, y1) = vp.start
    let (x2, y2) = vp.finish
    x1 = x2 || y1 = y2
    
[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let ventLines =
        File.ReadLines(fullPath)
        |> List.ofSeq
        |> List.map (fun r -> r.Split(" -> "))

    let startPoints =
        ventLines
        |> List.map (fun r -> (r.[0].Split(",") |> Array.map int))
        |> List.map (fun r -> (r.[0], r.[1]))

    let finishPoints =
        ventLines
        |> List.map (fun p -> (p.[1].Split(",") |> Array.map int))
        |> List.map (fun p -> (p.[0], p.[1]))

    let ventData: List<ventPoint> =
        (startPoints, finishPoints)
        ||> List.map2 (fun sP fP -> { start = sP; finish = fP })

    let straightLinesOnly = ventData |> List.filter xOrYMatch
    
    

    let straightLinePoints = generatePointsBetween straightLinesOnly []
    
    let diagonals = ventData |> List.except straightLinesOnly
    
    let diagonalPoints = generatePointsBetween diagonals []

    let straightOverlaps = straightLinePoints 
                        |> List.countBy id
                        |> List.filter (fun q -> (snd q) > 1)
                        
    let diagonalOverlaps = (straightLinePoints @ diagonalPoints) 
                        |> List.countBy id
                        |> List.filter (fun q -> (snd q) > 1)

                            
    printfn $"Points that overlap: %d{straightOverlaps.Length}"
    printfn $"Points that overlap with Diagonals: %d{diagonalOverlaps.Length}"
    0 // return an integer exit code
