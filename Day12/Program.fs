// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

// Define a function to construct a message to print

let isLower (st:string) =
    st.ToLower() = st
    
let appendNextPath (route:string) (connections: Map<string, string list>) =
    let routeParts = route.Split(",") |> List.ofArray
    let has2LowerCaves =
        routeParts
        |> List.filter(fun r -> isLower r)
        |> List.countBy id
        |> List.exists(fun (_,count)-> count > 1)
        
    let lastCave = routeParts |> List.last
    connections
    |> Map.find lastCave
    |> List.filter(fun nextStep -> nextStep <> "start")
    |> List.map(fun nextStep ->if lastCave = "end"
                                then route
                                else
                                    // we can have one instance where we repeat a lower case
                                    // after that you  can only enter lower case caves once
                                    if
                                       (has2LowerCaves)
                                       && ((routeParts |> List.contains nextStep) && isLower nextStep)
                                    then "" else route+","+nextStep)
    |> List.filter (fun x -> x <> "")

let rec buildRoutesUntilAllEnd caveConnections knownRoutes  =
        let nextGen =
            knownRoutes
            |> List.map(fun route -> caveConnections |> appendNextPath route )
            |> List.concat
            |> List.distinct
        
        if nextGen |> List.exists(fun route -> (route.Split(",")|> List.ofArray |> List.last) <> "end")
        then nextGen |> buildRoutesUntilAllEnd caveConnections
        else nextGen

[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data = File.ReadLines(fullPath)
                          |> List.ofSeq
                          |> List.map(fun txt -> txt.Split("-") |> List.ofArray)

    let caveConnections =
        data
        |> List.concat
        |> List.distinct
        |> List.map(fun cave -> cave, data// return a tuple of all connections to this cave
                                      |> List.filter(fun con -> con |> List.contains cave)
                                      |> List.concat
                                      |> List.except [cave])
        |> Map.ofList

 
            
    let routes = ["start"] |> buildRoutesUntilAllEnd caveConnections
                
            
    printfn $"answer: %d{routes |> List.length}" 
    0 // return an integer exit code
