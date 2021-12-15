// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
//      0:      1:      2:      3:      4:
//     aaaa    ....    aaaa    aaaa    ....
//    b    c  .    c  .    c  .    c  b    c
//    b    c  .    c  .    c  .    c  b    c
//     ....    ....    dddd    dddd    dddd
//    e    f  .    f  e    .  .    f  .    f
//    e    f  .    f  e    .  .    f  .    f
//     gggg    ....    gggg    gggg    ....
//
//      5:      6:      7:      8:      9:
//     aaaa    aaaa    aaaa    aaaa    aaaa
//    b    .  b    .  .    c  b    c  b    c
//    b    .  b    .  .    c  b    c  b    c
//     dddd    dddd    ....    dddd    dddd
//    .    f  e    f  .    f  e    f  .    f
//    .    f  e    f  .    f  e    f  .    f
//     gggg    gggg    ....    gggg    gggg
    // 1 uses 2 digits
// 2 uses 5 digits
// 3 uses 5 digits
    // 4 uses 4 digits
// 5 uses 5 digits
// 6 uses 6 digits
    // 7 uses 3 digits
    // 8 uses 7 digits
// 9 uses 6 digits

open System.IO

// Define a function to construct a message to print

[<EntryPoint>]
let main argv =
    // ten unique signal patterns
    // a pipe delimiter
    // four digit output value
    let fullPath = Path.Combine(__SOURCE_DIRECTORY__, "data.txt")
 
    let data = File.ReadLines(fullPath) |> List.ofSeq
    
    let outputValues = data
                        |> List.map(fun v -> v.Split("|").[1])
   
    let uniqueOutputValues = outputValues
                            |> List.map(fun v -> v.Split(" ") |> List.ofArray)
                            |> List.concat
                            |> List.filter(fun v -> v.Length = 2 || v.Length = 4 || v.Length = 3 || v.Length = 7)
                           
    
    printfn $"Simple values in the output %d{uniqueOutputValues |> Seq.length}"
    0 // return an integer exit code