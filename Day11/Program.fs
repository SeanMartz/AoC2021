// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO
type Position = int * int
type Energy = int
type OctopusMap = Map<Position, Energy>

let getNeighbors (x: int, y: int) (octoMap: OctopusMap)  =
    [ (-1, 1) // top left
      (0, 1) // directly up
      (1, 1) // top right
      (-1, 0) // left
      (1, 0) // right
      (-1, -1) // bottom left
      (0, -1) // directly down
      (1, -1) ] // bottom right
    |> List.map (fun (neighborX, neighborY) -> (x + neighborX, y + neighborY))
    |> List.choose
        (fun location ->
            match octoMap |> Map.tryFind location with
            | Some value -> Some(location, value)
            | None -> None)



let rec handleFlashes totalFlashCount flashedAlready (octopusMap: OctopusMap) : (OctopusMap * int) =
        let nextOcto =
            octopusMap
            |> Map.toList
            |> List.filter (fun (pos, _) -> flashedAlready
                                                |> Set.contains pos
                                                |> not) // filter out any that have already flashed
            |> List.tryFind(fun (pos, energyLevel) -> energyLevel > 9)
        
        match nextOcto with
        | Some (pos, level) ->
            //  If this causes an octopus to have an energy level greater than 9, it also flashes.
            // This process continues as long as new octopuses keep having their energy level increased beyond 9. (An octopus can only flash at most once per step.                        
            let octoMapWithNeighborsBumped =
                        octopusMap
                        |> getNeighbors pos
                        |> List.fold (fun octoMap (pos, energyLevel) ->
                                            let currentValue = octoMap |> Map.find pos
                                            octoMap |> Map.add pos (currentValue + 1)
                                    ) octopusMap
                        
            handleFlashes (totalFlashCount+1) (flashedAlready |> Set.add pos) octoMapWithNeighborsBumped

        | None ->
            //Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
            let newOctoMap =
                octopusMap
                |> Map.map( fun loc energy -> if flashedAlready |> Set.contains loc then 0 else energy)
            newOctoMap, totalFlashCount
            
            

let rec takeStep currentStep totalFlashes (octopusMap: OctopusMap): Map<int, int> =
    let newOctoMap, flashesThisStep =
        octopusMap
        //First, the energy level of each octopus increases by 1.
        |> Map.map(fun loc energy -> energy + 1)
        //Then, any octopus with an energy level greater than 9 flashes.
        |> handleFlashes 0 Set.empty
        
    if currentStep = 100 then
        totalFlashes
    else
        takeStep (currentStep+1) (totalFlashes |> Map.add currentStep flashesThisStep) newOctoMap 
    

[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data =
        File.ReadLines(fullPath)
        |> Array.ofSeq
        |> Array.map
            (fun x ->
                x
                |> Seq.toList
                |> Array.ofSeq
                |> Array.map string // string -> int conversion necessary otherwise i'll get the characters encoding number
                |> Array.map int)

    let octopusMap =
        [ for i, row in data |> Array.indexed do
              for j, point in row |> Array.indexed -> (i, j), point ]
        |> Map.ofList
        
        
    let stepsAndFlashes = takeStep 0 Map.empty octopusMap
    let answer = stepsAndFlashes
                 |> Map.toList
                 |> List.map(fun x -> snd x)
                 |> List.sum
                 
    printf $"Answer: %d{answer}"


    0 // return an integer exit code
