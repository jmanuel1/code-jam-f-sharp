open CLIArguments
open Output

type Solution = | Solution of seq<TestCaseOutput>

let usageMessage =
    "This program takes a single argument --problem which can take the following values\n" +
    "rank-and-file : run a solution to the 2016 Round 1a 'Rank and File' problem\n\n" +
    "Input, just like in the real CodeJam, is fed through standard input."
let printUsage () = printfn "%s" usageMessage

let solveAndOutput (Solution solution) =
    for output in solution do printCaseOutput output

[<EntryPoint>]
let main argv = 
    let args = Array.toList argv |> parseArgs
    let solution =
        match args with
        | Args { problem = RankAndFile } -> _2016_1a_rank_and_file.solution |> Solution
        | NoValidArgs ->
            printUsage()
            exit 1 // just exit with a non-zero integer instead of throwing an exception
    // run solution and print output
    solveAndOutput solution
    0 // return an integer exit code
