// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

// Define a function to construct a message to print

let rec determineDistance (distances: list<int>) (positions: list<int>) (mapping: Map<int, int>) : Map<int, int> =
    match distances with
    | head :: tail ->
        determineDistance tail positions mapping
        |> Map.add
            head
            (positions
             |> List.map (fun pos -> abs (head - pos))
             |> List.sum)
    | [] -> mapping

let rec determineDistancePartTwo (distances: list<int>) (positions: list<int>) (mapping: Map<int, int64>) : Map<int, int64> =
    match distances with
    | head :: tail ->
        determineDistancePartTwo tail positions mapping
        |> Map.add
            head
            (positions
             |> List.map (fun pos -> [0..abs(head - pos)] |> List.sum)
             |> List.map int64
             |> List.sum)
    | [] -> mapping


[<EntryPoint>]
let main argv =
    
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let values = (File.ReadLines(fullPath) |> Seq.head).Split(",") |> List.ofArray |> List.map int
                        
    let distinctDistances = [0..values |> List.max]

    let distanceMap = determineDistance distinctDistances values Map.empty

    let shortestDistance =
        distanceMap
        |> Map.toList
        |> List.sortBy (fun pd -> snd pd)
        |> List.head


   
    printfn $"Horizontal Position %d{fst shortestDistance} has Shortest Distance: %d{snd shortestDistance}"
        
        
    let distanceMapPartTwo =  determineDistancePartTwo distinctDistances values Map.empty
    let shortestDistancePartTwo = distanceMapPartTwo
                                    |> Map.toList
                                    |> List.sortBy (fun pd -> snd pd)
                                    |> List.head
        
    printfn $"Horizontal Position Part Two %d{fst shortestDistancePartTwo} has Shortest Distance: %d{snd shortestDistancePartTwo}"


    0 // return an integer exit code