// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO


let closesChunk char1 char2 : bool =
    match char1 with
    | "(" when char2.Equals ")" -> true
    | "[" when char2.Equals "]" -> true
    | "{" when char2.Equals "}" -> true
    | "<" when char2.Equals ">" -> true
    | _ -> false


let rec findInvalidCharacters stack (listOfChars: string list) : string option =
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
                Some currentChar
    | [] -> if stack.IsEmpty then None else Some ""

[<EntryPoint>]
let main argv =
    let fullPath =
        Path.Combine(__SOURCE_DIRECTORY__, "data.txt")

    let data =
        File.ReadLines(fullPath)
        |> List.ofSeq
        |> List.map (fun x -> x |> List.ofSeq |> List.map string)

    let corruptRows =
        data
        |> List.choose (fun row -> findInvalidCharacters [] row)

    let corruptChars =
        corruptRows
        |> List.filter (fun ltr -> ltr.Length > 0)

    let pointValues =
        Map [ (")", 3)
              ("]", 57)
              ("}", 1197)
              (">", 25137) ]

    let points =
        corruptChars
        |> List.map (fun badChar -> pointValues |> Map.find badChar)
        |> List.sum

    printfn $"Answer %d{points}"
    0 // return an integer exit code
