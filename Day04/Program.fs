// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
open System.IO
type bingoNumber = { value: string; chosen: bool }
type bingoRow = List<bingoNumber>

type bingoBoard =
    struct
        val rows: List<bingoRow>
        new(rows) = { rows = rows }
    end

type winningBoard = { board: bingoBoard; winningNumber: string }


let rec parseRowsIntoBoards (bingoBoards: list<bingoBoard>) (data: list<string>) : List<bingoBoard> =
    match data with
    | head :: tail when head = "" ->
        let newBingoBoard = bingoBoard ([])
        parseRowsIntoBoards ([ newBingoBoard ] @ bingoBoards) tail // add a new board
    | head :: tail ->
        let newRow =
            List.ofArray (head.Split(" "))
            |> List.filter (fun x -> x <> "")
            |> List.map (fun x -> { value = x; chosen = false })

        let firstBingoBoardConcatWithNewRows =
            bingoBoard (bingoBoards.Head.rows @ [ newRow ])

        parseRowsIntoBoards
            ([ firstBingoBoardConcatWithNewRows ]
             @ bingoBoards.Tail)
            tail
    | _ -> bingoBoards


let applyCall (call: string) (boards: list<bingoBoard>) : list<bingoBoard> =
    boards
    |> List.map
        (fun board ->
            bingoBoard (
                board.rows
                |> List.map
                    (fun row ->
                        row
                        |> List.map
                            (fun number ->
                                if number.value = call then
                                    { value = number.value; chosen = true }
                                else
                                    { value = number.value
                                      chosen = number.chosen }))
            ))


let checkForRowWin (board: bingoBoard) : bool =
    board.rows
    |> List.map (fun row -> row |> List.forall (fun bn -> bn.chosen))
    |> List.exists (fun winner -> winner)

let checkForColumnWin (board: bingoBoard) : bool =
    [ 0 .. 4 ]
    |> List.map
        (fun col ->
            board.rows
            |> List.map (fun row -> row.Item(col))
            |> List.forall (fun bn -> bn.chosen))
    |> List.exists (fun winner -> winner)

let isWinningBoard (board: bingoBoard) : bool =
    let rowWin = checkForRowWin board
    let columnWin = checkForColumnWin board
    rowWin || columnWin

let rec playGame calls boards (winningBoards: list<winningBoard>) : list<winningBoard> =
    match calls with
    | call :: upcomingCalls ->
        let markedBoards = boards
                           |> applyCall call
                           
        let winners : List<winningBoard> = winningBoards @ (markedBoards
                                            |> List.filter isWinningBoard
                                            |> List.map(fun b -> {board = b; winningNumber = call }))
                        
                        
        let boardsToKeepPlaying = markedBoards |> List.filter (fun board -> not (isWinningBoard board))
        playGame upcomingCalls boardsToKeepPlaying winners
    | [] -> winningBoards
    | _ -> failwith "calls exhausted, no boards won."

let sumUncalledNumbers (board: bingoBoard ) : int =
    board.rows
    |> List.concat
    |> List.filter (fun bn -> not bn.chosen)
    |> List.map (fun n -> n.value)
    |> List.map int
    |> List.sum

[<EntryPoint>]
let main argv =
    let callsPath =
        Path.Combine(__SOURCE_DIRECTORY__, "calls.txt")

    let boardsPath =
        Path.Combine(__SOURCE_DIRECTORY__, "boards.txt")

    let callsCsv = File.ReadLines(callsPath) |> List.ofSeq
    let calls = callsCsv.Head.Split(",") |> List.ofArray


    let boards =
        File.ReadLines(boardsPath)
        |> List.ofSeq
        |> parseRowsIntoBoards [ bingoBoard ([]) ]
        |> List.rev

    let winningBoards = playGame calls boards []
    let sum = sumUncalledNumbers winningBoards.Head.board
    let answer = sum * int (winningBoards.Head.winningNumber)
    let lastWinner = (List.rev winningBoards).Head
    let partTwoAnswer = (sumUncalledNumbers lastWinner.board) * int lastWinner.winningNumber

    printfn $"Answer: %d{answer}"
    printfn $"Part Two Answer: %d{partTwoAnswer}"
    0 // return an integer exit code
