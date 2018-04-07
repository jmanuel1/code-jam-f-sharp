module CLIArguments
    type Problem = | RankAndFile
    type Arguments = {problem:Problem}
    type ParsedArgs = Args of Arguments | NoValidArgs

    // don't do dispatch to solution modules here, that's beyond the scope of argument parsing

    let rec parseArgs args =
        match args with
        | "--problem"::"rank-and-file"::_ -> Args { problem = RankAndFile }
        | unrecognized::rest ->
            eprintfn "Unrecognized command line argument: %s" unrecognized
            parseArgs rest
        | [] -> NoValidArgs