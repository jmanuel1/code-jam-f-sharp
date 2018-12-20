module _2016_1a_rank_and_file
    open Input
    open Output

    type TestData = {n:int; lists:int list list}

    let testCaseParser line =
        let n = int line
        let rec testCaseListsParser linesLeft lists (line:string) =
            let lst = line.Split(' ') |> Array.map int |> Array.toList
            let lists' = List.append lists [lst]
            match linesLeft with
            | 1 -> { n = n; lists = lists' } |> Case |> ParsedTestCase 
            | _ ->
                testCaseListsParser (linesLeft - 1) lists' |> Parser
        testCaseListsParser (2*n - 1) [] |> Parser

    let generateGrid lists =
        let areCompatible list index grid =
            // check that list matches what's currently in the row
            let row = List.item index grid 
            let doesMatch = List.forall2 (fun listEl rowEl -> 
                (rowEl = 0) || (listEl = rowEl)) list row
            // check that order is not violated
            let getRow index =
                let size = List.length list
                match index with
                | i when i = size || i = -1 -> List.replicate size 0
                | _ -> grid.[index]
            let leftOrder = index - 1 |> getRow |> List.forall2 ( > ) list
            let doesFitOrder = leftOrder &&
                               (index + 1 |> getRow |> List.forall2 (fun a b ->
                                a < b || b = 0) list)
            doesMatch && doesFitOrder
        let produceMoreCompleteGrids list grid =
            List.mapi (fun index _ ->
                    if areCompatible list index grid then
                        Some <| List.concat [
                            List.truncate index grid
                            [list]
                            grid.[index+1..]
                        ]
                    else None
                    ) grid
                |> List.choose (fun newGrid -> newGrid)
        let isComplete = List.forall (fun n -> n > 0) |> List.forall 
        let rec generateGridInner possibleGrids lists =
            match lists with
            | [] -> List.filter isComplete possibleGrids |> List.head
            | _ ->
                let moreCompleteGrids = [
                    for grid in possibleGrids do
                        // possible rows
                        yield! produceMoreCompleteGrids lists.[0] grid;
                        // possible columns
                        yield! List.transpose grid 
                            |> produceMoreCompleteGrids lists.[0] 
                            |> List.map List.transpose
                ]
                generateGridInner moreCompleteGrids lists.[1..]
        let length = List.head lists |> List.length
        let initialGrid = List.replicate length 0 |> List.replicate length
        generateGridInner [initialGrid] lists

    let findMissingList given grid =
        // default list sorting works
        let allLists = List.transpose grid |> List.append grid |> List.sort 
        let sortedGiven = List.sort given
        let firstDifferentList =
            sortedGiven 
            |> List.zip (List.truncate (List.length sortedGiven) allLists)
            |> List.tryPick (fun (allList, givenList) ->
                match allList = givenList with
                | true -> None
                | false -> Some allList)
        match firstDifferentList with
        | Some list -> list
        | None -> List.last allLists

    // actually solve the problem
    let solution =
        seq {
            for case in allTestCases testCaseParser do
                // construct grid
                let lists = case.case.lists
                let grid = generateGrid lists

                let missingList = findMissingList lists grid
                yield { 
                    caseNumber = case.number
                    output = List.map string missingList |> String.concat " " 
                } 
        }

