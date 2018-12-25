﻿module CLIArguments
    type Problem = TheLastWord | RankAndFile | CoinJam | None
    type Arguments = {problem:Problem; help:bool}
    type ParsedArgs = 
        | Args of Arguments
        | NoArgs
        | RepeatedArg of string
        | MissingValue of string
        | TooManyArgs
        | BadValue of string * string
        | BadArg of string

    (* don't do dispatch to solution modules here, that's beyond the scope of 
       argument parsing *)

    /// Parse command line arguments from a list of arguments.
    /// The arguments recognized are as follows:
    /// * --problem <name> : Run the solution to the problem named <name>. 
    /// <name> can be any of:
    ///     * rank-and-file
    ///     * the-last-word
    ///     * coin-jam
    ///     If any other name is given, BadValue ("--problem", "<name>") will 
    ///     be returned. If multiple valid --problem arguments are given, 
    ///     RepeatedArg "--problem" will be returned.
    /// * --help : Print a help message.
    /// If no options are given, NoArgs will be returned. --problem must be the
    /// only option, else TooManyArgs will be returned. If an unrecognized 
    /// option is given, BadArg "<arg>" will be returned.
    let rec parseArgs args =
        match args with
        | "--problem"::rest ->
            match rest with
            | problem::rest ->
                let argRepresentation = 
                    match problem with
                    | "rank-and-file" -> 
                        Args { problem = RankAndFile; help = false }
                    | "the-last-word" -> 
                        Args { problem = TheLastWord; help = false }
                    | "coin-jam" -> Args { problem = CoinJam; help = false }
                    | _ -> BadValue ("--problem", problem)
                match parseArgs rest with
                | Args { problem = _ }
                | RepeatedArg "--problem"
                | BadValue ("--problem", _)
                | MissingValue "--problem" -> RepeatedArg "--problem"
                | NoArgs -> argRepresentation
                | _ -> TooManyArgs
            | [] -> MissingValue "--problem"
        | "--help"::_ | "help"::_ -> Args { problem = None; help = true }
        | unrecognized::_ ->
            BadArg unrecognized
        | [] -> NoArgs