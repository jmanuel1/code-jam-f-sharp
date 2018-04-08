module CLIArguments
    type Problem = | RankAndFile
    type Arguments = {problem:Problem}
    type ParsedArgs = 
        | Args of Arguments
        | NoValidArgs
        | RepeatedArg of string
        | MissingValue of string

    // don't do dispatch to solution modules here, that's beyond the scope of argument parsing

    /// Parse command line arguments from a list of arguments.
    /// The arguments recognized are as follows:
    /// * --problem <name> : Run the solution to the problem named <name>. <name> can be any of:
    ///     * rank-and-file
    ///     If any other name is given, an error message will be printed to standard error and
    ///     the --problem argument will be ignored. If multiple valid --problem arguments are
    ///     given, an error message will be printed to standard error and RepeatedArg "--problem"
    ///     will be returned.
    /// If no recognized options are parsed, NoValidArgs will be returned.
    let rec parseArgs args =
        match args with
        | "--problem"::rest ->
            match rest with
            | "rank-and-file"::rest ->
                match parseArgs rest with
                | Args { problem = _ } -> RepeatedArg "--problem"
                | _ -> Args { problem = RankAndFile }
            | unrecognized::rest ->
                eprintfn "Unrecognized problem name passed to --problem: %s" unrecognized
                parseArgs rest
            | [] -> MissingValue "--problem"
        | unrecognized::rest ->
            eprintfn "Unrecognized command line argument: %s" unrecognized
            parseArgs rest
        | [] -> NoValidArgs