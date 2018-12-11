open CLIArguments
open Output

type Solution = | Solution of seq<TestCaseOutput>

let usageMessage =
    "This program takes a single argument --problem which can take the following values\n" +
    "rank-and-file : run a solution to the 2016 Round 1a 'Rank and File' problem\n" +
    "the-last-word : run a solution to the 2016 Round 1a 'The Last Word' problem\n\n" +
    "Input, just like in the real CodeJam, is fed through standard input."
let printUsage () = printfn "%s" usageMessage

let solveAndOutput (Solution solution) =
    for output in solution do printCaseOutput output

let handleArgParsingError err =
    let message =
        match err with
        | NoArgs ->
            "No arguments were passed.\n"
        | MissingValue arg ->
            sprintf "Argument %s was not given a value.\n" arg
        | RepeatedArg arg ->
            sprintf "Argument %s must not be given more than once.\n" arg
        | BadArg arg ->
            sprintf "Argument %s is not recognized.\n" arg
        | BadValue (arg, value) ->
            sprintf "Argument %s was passed with the unrecognized value %s.\n" arg value
        | TooManyArgs ->
            "Too many arguments were passed.\n"
        | Args _ ->
            // the code is quite wrong if it gets here
            let errMsg =
                "CLI arguments were parsed but treated as an error. " +
                "Perhaps a match expression is wrong somewhere (in main?)?"
            invalidArg "err" errMsg
    eprintfn "%s" message
    printUsage()
    exit 1

[<EntryPoint>]
let main argv = 
    let args = Array.toList argv |> parseArgs
    let solution =
        match args with
        | Args { problem = RankAndFile } -> _2016_1a_rank_and_file.solution |> Solution
        | Args { problem = TheLastWord } -> _2016_1a_the_last_word.solution |> Solution
        | err -> handleArgParsingError err
    // run solution and print output
    solveAndOutput solution
    0 // return an integer exit code
