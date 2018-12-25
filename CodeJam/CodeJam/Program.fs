open CLIArguments
open Output

type Solution = | Solution of seq<TestCaseOutput>

let usageMessage =
    "This program can take a single argument --problem which can take the " +
    "following values\n" +
    "rank-and-file : run a solution to the 2016 Round 1a 'Rank and File' " +
    "problem\n" +
    "the-last-word : run a solution to the 2016 Round 1a 'The Last Word' " +
    "problem\n" + 
    "coin-jam : run a solution to the 2016 Qualification Round 'Coin Jam' " +
    "problem\n\n" +
    "If no arguments are given, or the --help/help/-h argument is given, " +
    "then this help message is printed.\n\n" +
    "Input, just like in the real CodeJam, is fed through standard input."
let printUsage () = printfn "%s" usageMessage

let solveAndOutput (Solution solution) =
    for output in solution do printCaseOutput output

let handleArgParsingError err =
    let message =
        match err with
        | MissingValue arg ->
            sprintf "Argument %s was not given a value.\n" arg
        | RepeatedArg arg ->
            sprintf "Argument %s must not be given more than once.\n" arg
        | BadArg arg ->
            sprintf "Argument %s is not recognized.\n" arg
        | BadValue (arg, value) ->
            (sprintf "Argument %s was passed with the unrecognized value %s.\n"
                arg value)
        | TooManyArgs ->
            "Too many arguments were passed.\n"
        (* Purposely not match Args _ b/c it wouldn't make sense to do so. *)
    eprintfn "%s" message
    printUsage()
    exit 1

[<EntryPoint>]
let main argv = 
    let args = Array.toList argv |> parseArgs
    match args with
    | Args { problem = problem; help = false } ->
        let solution = 
            match problem with
            | RankAndFile -> 
                _2016_1a_rank_and_file.solution |> Solution
            | TheLastWord -> 
                _2016_1a_the_last_word.solution |> Solution
            | CoinJam -> _2016_qu_coin_jam.solution |> Solution
        // run solution and print output
        solveAndOutput solution
    | NoArgs | Args { help = true } -> printUsage()
    | err -> handleArgParsingError err
    0 // return an integer exit code
