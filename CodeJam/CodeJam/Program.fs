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
        | NoArgs ->
            printUsage()
            exit 1 // just exit with a non-zero integer instead of throwing an exception
        | MissingValue arg ->
            eprintfn "Argument %s was not given a value.\n" arg
            printUsage()
            exit 1
        | RepeatedArg arg ->
            eprintfn "Argument %s must not be given more than once.\n" arg
            printUsage()
            exit 1
        | BadArg arg ->
            eprintfn "Argument %s is not recognized.\n" arg
            printUsage()
            exit 1
        | BadValue (arg, value) ->
            eprintfn "Argument %s was passed with the unrecognized value %s.\n" arg value
            printUsage()
            exit 1
        | TooManyArgs ->
            eprintfn "Too many arguments were passed.\n"
            printUsage()
            exit 1
    // run solution and print output
    solveAndOutput solution
    0 // return an integer exit code
