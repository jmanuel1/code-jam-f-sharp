module _2016_1a_the_last_word
    open Input
    open Output

    type TestData = {str:string}

    let testCaseParser line =
        { str = line } |> Case |> ParsedTestCase

    let lastWordBuilder (str:string) =
        let rec lastWordBuilderInner str (lastWord:string) =
            match str with
            | "" -> lastWord
            | _ -> 
                if str.[0] >= lastWord.[0] then
                    lastWordBuilderInner str.[1..] (str.[..0] + lastWord)
                else
                    lastWordBuilderInner str.[1..] (lastWord + str.[..0])
        
        lastWordBuilderInner str.[1..] str.[..0]

    let solution = 
        seq {
            for case in allTestCases testCaseParser do
                let str = case.case.str
                let lastWord = lastWordBuilder str
                yield { caseNumber = case.number; output = lastWord } }
