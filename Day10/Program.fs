// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO


let closesChunk char1 char2 : bool =
    match char1 with
    | "(" when char2.Equals ")" -> true
    | "[" when char2.Equals "]" -> true
    | "{" when char2.Equals "}" -> true
    | "<" when char2.Equals ">" -> true
    | _ -> false


let rec findInvalidCharacters stack (listOfChars: string list) : string list option =
    // if we have a characters left to parse
    //// if it's an opening character
    ////// add it to the stack, and keep parsing
    //// if it's not an opening character
    ////// then if it is a valid closing character to the top of the stack
    //////// then pop off stack and keep searching
    //// else, return the bad character and call this chunk invalid.
    // if we're out of list
    //// then if theres anything on the stack chunk is invalid
    match listOfChars with
    | currentChar :: restOfLine ->
        match currentChar with
        | "("
        | "["
        | "{"
        | "<" -> findInvalidCharacters (currentChar :: stack) restOfLine
        | _ ->
            if currentChar |> closesChunk stack.Head then
                findInvalidCharacters stack.Tail restOfLine
            else
                Some [ currentChar ]
    | [] ->
        if stack.IsEmpty then
            None
        else
            Some stack

[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data =
        File.ReadLines(fullPath)
        |> List.ofSeq
        |> List.map (fun x -> x |> List.ofSeq |> List.map string)

    let invalidRows =
        data
        |> List.choose (fun row -> findInvalidCharacters [] row)

    let corruptChars =
        invalidRows
        |> List.filter (fun ltrs -> ltrs.Length = 1)
        |> List.map (fun ltrs -> ltrs.Head)

    let corruptPoints =
        Map [ (")", 3)
              ("]", 57)
              ("}", 1197)
              (">", 25137) ]

    let corruptSum =
        corruptChars
        |> List.map (fun badChar -> corruptPoints |> Map.find badChar)
        |> List.sum

    printfn $"Answer %d{corruptSum}"

    let unfinishedPoints =
        Map [ (")", bigint 1)
              ("]", bigint 2)
              ("}", bigint 3)
              (">", bigint 4) ]

    let swapClosingChars chr =
        match chr with
        | "(" -> ")"
        | "[" -> "]"
        | "{" -> "}"
        | "<" -> ">"
        
    let scores =
        invalidRows
        |> List.filter (fun ltrs -> ltrs.Length > 1)
        |> List.map (fun ltrs -> ltrs |> List.map swapClosingChars)
        |> List.map (fun lst -> lst |> List.map (fun chr -> unfinishedPoints |> Map.find chr))
        |> List.map (fun scores -> scores |> List.reduce(fun score pointValue -> (score * bigint 5) + pointValue))
        |> List.sort
        
    let answer2 = scores.[scores.Length/2] 
        
    printfn $"Answer2 %A{answer2}"

    0 // return an integer exit code
