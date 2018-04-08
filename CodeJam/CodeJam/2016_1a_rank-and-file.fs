module _2016_1a_rank_and_file
    open Input
    open Output

    type TestData = {n:int; lists:int list list}

    let testCaseParser line =
        let n = int line
        let rec testCaseListsParser linesLeft lists (line:string) =
            match linesLeft with
            | 1 -> { n = n; lists = lists } |> Case |> ParsedTestCase 
            | _ ->
                let lst = line.Split(' ') |> Array.map int |> Array.toList
                List.append lists [lst] |> testCaseListsParser (linesLeft - 1) |> Parser
        testCaseListsParser (2*n - 1) [] |> Parser

    let key =
        // ** does not support ints
        List.rev >> List.mapi (fun power n -> n*(pown 2500 power)) >> List.reduce ( + )
 
    let rec generateGrid grid lists =
        match (grid, lists) with
        | ([], first::rest) -> generateGrid [first] rest
        | (_, l::rest) ->
            if List.exists2 ( = ) l (List.last grid) then
                generateGrid grid rest else
                generateGrid (List.append grid [l]) rest
        | (_, []) -> grid

    // actually solve the problem
    let solution =
        seq {
            for case in allTestCases testCaseParser do
                // construct grid
                let n = case.case.n
                let lists = List.sortBy key case.case.lists
                let grid = generateGrid [] lists
                // printfn "%A" grid

                // generate list of columns
                let cols =
                    [ for i in { 0 .. n - 1 } do
                        yield List.map (List.item i) grid ]
                // printfn "%A" cols

                let y = (List.filter (fun l -> not (List.contains l lists)) (List.append cols grid)).[0]
                yield { caseNumber = case.number; output = List.map string y |> String.concat " " } }

