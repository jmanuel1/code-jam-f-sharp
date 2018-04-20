module CLIArguments
    type Problem = | RankAndFile
    type Arguments = {problem:Problem}
    type ParsedArgs = 
        | Args of Arguments
        | NoArgs
        | RepeatedArg of string
        | MissingValue of string
        | TooManyArgs
        | BadValue of string * string
        | BadArg of string

    // don't do dispatch to solution modules here, that's beyond the scope of argument parsing

    /// Parse command line arguments from a list of arguments.
    /// The arguments recognized are as follows:
    /// * --problem <name> : Run the solution to the problem named <name>. <name> can be any of:
    ///     * rank-and-file
    ///     If any other name is given, BadValue ("--problem", "<name>") will be returned. If
    ///     multiple valid --problem arguments are given, RepeatedArg "--problem"
    ///     will be returned.
    /// If no options are given, NoArgs will be returned. --problem must be the
    /// only option, else TooManyArgs will be returned. If an unrecognized option is given,
    /// BadOption "<opt>" will be returned.
    let rec parseArgs args =
        match args with
        | "--problem"::rest ->
            match rest with
            | "rank-and-file"::rest ->
                match parseArgs rest with
                | Args { problem = _ }
                | RepeatedArg "--problem"
                | BadValue ("--problem", _)
                | MissingValue "--problem" -> RepeatedArg "--problem"
                | NoArgs -> Args { problem = RankAndFile }
                | _ -> TooManyArgs
            | unrecognized::_ ->
                BadValue ("--problem", unrecognized) //eprintfn "Unrecognized problem name passed to --problem: %s" unrecognized
            | [] -> MissingValue "--problem"
        | unrecognized::_ ->
            BadArg unrecognized //eprintfn "Unrecognized command line argument: %s" unrecognized
        | [] -> NoArgs