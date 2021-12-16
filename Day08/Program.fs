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



// positions: t, tl, tr, m, bl, br, b

// 2 uses 5 digits
// 3 uses 5 digits
// 5 uses 5 digits

// 0 uses 6 digits
// 6 uses 6 digits
// 9 uses 6 digits


open System
open System.IO

// Define a function to construct a message to print


let whereLength num (lst: string list) : string list =
    lst |> List.filter (fun x -> x.Length = num)

let buildMap (inputValues: list<string>) : Map<Set<char>, string> =
    // figure out the easy ones by length
    let one =
        inputValues |> whereLength 2 |> List.head

    let four =
        inputValues |> whereLength 4 |> List.head

    let seven =
        inputValues |> whereLength 3 |> List.head

    let eight =
        inputValues |> whereLength 7 |> List.head

    // use set logic to handle differences cleanly
    // need to use segments to figure out missing numbers
    let oneSegments = one |> Set.ofSeq


    // of 2,3, and 5 (five digits)
    let fiveDigitNumbers = inputValues |> whereLength 5
    // 3 is the only item that contains the same segments as 1
    let three =
        fiveDigitNumbers
        |> List.filter (fun x -> Set.isSubset oneSegments (x |> Set.ofSeq))
        |> List.head

    let fourSegments = four |> Set.ofSeq
    let topLeftAndMidSegments = Set.difference fourSegments oneSegments

    // of 2,3, and 5 (five digits)
    // 5 is the only item that has the top left and mid segment
    let five =
        fiveDigitNumbers
        |> List.filter (fun x -> Set.isSubset topLeftAndMidSegments (x |> Set.ofSeq))
        |> List.head

    // twos are whatever is left with 5 digits
    let two =
        fiveDigitNumbers
        |> List.filter (fun x -> x <> three)
        |> List.filter (fun x -> x <> five)
        |> List.head

    let sevenSegments = seven |> Set.ofSeq
    // six is the only 6 digit number that doesnt contains the 7s' segments and does contain the topLeft_Mid_Segment
    let sixDigitNumbers = inputValues |> whereLength 6

    let six =
        sixDigitNumbers
        |> List.filter (fun x -> not (Set.isSubset sevenSegments (x |> Set.ofSeq)))
        |> List.filter (fun x -> Set.isSubset topLeftAndMidSegments (x |> Set.ofSeq))
        |> List.head

    // 9 has the top left and middle,
    let nine =
        inputValues
        |> whereLength 6
        |> List.filter (fun x -> x <> six)
        |> Seq.filter (fun x -> Set.isSubset topLeftAndMidSegments (x |> Set.ofSeq))
        |> Seq.head

    // leaving only 0s in the 6 digit list.
    let zero =
        inputValues
        |> whereLength 6
        |> List.filter (fun x -> x <> six)
        |> List.filter (fun x -> x <> nine)
        |> List.head

    Map.empty
    |> Map.add (zero|> Set.ofSeq) "0"
    |> Map.add (one|> Set.ofSeq) "1"
    |> Map.add (two|> Set.ofSeq) "2"
    |> Map.add (three|> Set.ofSeq) "3"
    |> Map.add (four |> Set.ofSeq)"4"
    |> Map.add (five|> Set.ofSeq) "5"
    |> Map.add (six|> Set.ofSeq) "6"
    |> Map.add (seven |> Set.ofSeq)"7"
    |> Map.add (eight|> Set.ofSeq) "8"
    |> Map.add (nine |> Set.ofSeq) "9"

let decode (map: Map<Set<char>, string>) (str: string) : string =
    str.Trim()
        .Split(" ")
    |> List.ofArray
    |> List.map(fun x -> x|> Set.ofSeq)
    |> List.map (fun x -> map.Item x)
    |> String.concat ""


[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data = File.ReadLines(fullPath) |> List.ofSeq

    let splitData =
        data
        |> List.map (fun x -> x.Split("|") |> List.ofArray)

    let outputValues =
        splitData
        |> List.map (fun x -> x.Tail |> List.head)
        |> List.map (fun v -> v.Split(" ") |> List.ofArray)
        |> List.concat

    let uniqueOutputValues =
        outputValues
        |> List.filter
            (fun v ->
                v.Length = 2
                || v.Length = 4
                || v.Length = 3
                || v.Length = 7)

    printfn $"Simple values in the output %d{uniqueOutputValues |> Seq.length}"

    // part 2

    let q =
        splitData
        |> List.map (fun x ->
            (buildMap (x.Head.Split(" ") |> List.ofArray), x))
        |> List.map (fun x -> decode (fst x) (snd x).Tail.Head)
        |> List.map int
        |> List.sum
 
    printfn $"%d{q}"

    0 // return an integer exit code
