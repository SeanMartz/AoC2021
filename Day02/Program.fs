// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO
// Define a function to construct a message to print        
let sumInstruction (data: string list) (instruction: string) : int =
     data
        |> List.filter (fun inst -> inst.Contains(instruction))
        |> List.map (fun inst -> inst.Split(" "))
        |> List.map (fun inst -> inst.[1])
        |> List.map int
        |> List.sum

let parseInstructions (data: string list) : (int * int) =
    let x = sumInstruction data "forward"
    let up = sumInstruction data "up"
    let down = sumInstruction data "down"
    let y = down - up
    (x,y)
    
[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "instructions.txt")

    let data = File.ReadLines(fullPath) |> List.ofSeq

    let x, y = parseInstructions data
    let product = x * y
    printfn "X:%d, Y:%d" x y
    printfn "Product: %d" product
    0 // return an integer exit code
