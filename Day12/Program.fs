// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

// Define a function to construct a message to print

let isLower (st:string) =
    st.ToLower() = st
    
let appendNextPath (route:string) (connections: Map<string, string list>) =
    let routeParts = route.Split(",") |> List.ofArray
    let lastCave = routeParts |> List.last
    connections
    |> Map.find lastCave
    |> List.map(fun nextStep ->if lastCave = "end"
                                then route
                                else if (routeParts |> List.contains nextStep) && isLower nextStep
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
        |> List.concat // flatten
        |> List.distinct// get distinct
        |> List.map(fun cave -> cave, data// return a tuple of all connections to this cave
                                      |> List.filter(fun con -> con |> List.contains cave)
                                      |> List.concat
                                      |> List.except [cave])
        |> Map.ofList

 
            
    let routes = ["start"] |> buildRoutesUntilAllEnd caveConnections
                
            
    printfn "Hello world" 
    0 // return an integer exit code
