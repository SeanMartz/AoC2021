// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

// Define a function to construct a message to print
type ventPoint = { start: int * int; finish: int * int }

let rec generatePointsBetween points pointList : list<(int * int)> =
    match points with
    | head :: tail ->
        let xs =
            if fst head.start < fst head.finish then
                [ (fst head.start) .. (fst head.finish) ]
            else
                [ (fst head.finish) .. (fst head.start) ]

        let ys =
            if snd head.start < snd head.finish then
                [ (snd head.start) .. (snd head.finish) ]
            else
                [ (snd head.finish) .. (snd head.start) ]

        let newPoints =
            if xs.Length = 1 then
                ((Array.create ys.Length xs.Head |> List.ofArray), ys)
                ||> List.map2 (fun x y -> (x, y))
            else
                ((Array.create xs.Length ys.Head |> List.ofArray), xs)
                ||> List.map2 (fun y x -> (x, y))

        generatePointsBetween tail (pointList @ newPoints)
    | [] -> pointList


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

    let maxX =
        ventData
        |> List.map (fun vp -> [ fst vp.start ] @ [ fst vp.finish ])
        |> List.concat
        |> List.max

    let maxY =
        ventData
        |> List.map (fun vp -> [ snd vp.start ] @ [ snd vp.finish ])
        |> List.concat
        |> List.max

    let straightLinesOnly =
        ventData
        |> List.filter
            (fun vp ->
                let (x1, y1) = vp.start
                let (x2, y2) = vp.finish
                x1 = x2 || y1 = y2)

    let fullPointList = generatePointsBetween straightLinesOnly []

    let occurences = fullPointList
                     |> List.countBy id
                     |> List.filter (fun q -> (snd q) > 1)

                            
    printfn $"Points that overlap: %d{occurences.Length}"
    0 // return an integer exit code
