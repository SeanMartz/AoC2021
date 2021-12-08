// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO
// Define a function to construct a message to print
let sumInstruction (data: string list) (instruction: string) : int =
    data
    |> List.filter (fun inst -> inst.Contains(instruction))
    |> List.map (fun inst -> inst.Split(" "))
    |> List.map (fun inst -> inst.[1])
    |> List.map int
    |> List.sum

let parseInstructions (data: string list) : (int * int * int) =
    let forward = sumInstruction data "forward"
    let up = sumInstruction data "up"
    let down = sumInstruction data "down"

    (forward, up, down)

type Position =
    struct
        val horizontal: int
        val aim: int
        val depth: int

        new(horizontal: int, aim: int, depth: int) =
            { horizontal = horizontal
              aim = aim
              depth = depth }
    end

let rec moveSubmarine (commandList: string list) (current: Position) : Position =
    match commandList with
    | head :: tail ->
        let direction = head.Split(" ").[0]
        let value = (int (head.Split(" ").[1]))

        match direction with
        | "forward" -> moveSubmarine tail (Position(current.horizontal + value, current.aim, current.depth + (current.aim * value)))
        | "up" -> moveSubmarine tail (Position(current.horizontal, current.aim - value, current.depth))
        | "down" -> moveSubmarine tail (Position(current.horizontal, current.aim + value, current.depth))
        | _ -> failwith "todo"
    | _ -> current


[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "instructions.txt")

    let data = File.ReadLines(fullPath) |> List.ofSeq

    let fwd, up, down = parseInstructions data
    let depth = down - up
    let product = fwd * depth
    printfn $"Fwd:%d{fwd}, Depth:%d{depth}"
    printfn $"Product: %d{product}"

    // part 2
    let finalPosition = moveSubmarine data (Position(0, 0, 0))
    
    let newProduct = finalPosition.horizontal * finalPosition.depth
    printfn $"New Product: %d{newProduct}"
    0 // return an integer exit code
