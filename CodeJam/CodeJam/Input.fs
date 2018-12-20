module Input
    type NumTestCases = | NumberOfCases of int
    let numCasesToInt (NumberOfCases t) = t

    type TestCase<'a> = | Case of 'a
    let extractCase (Case case) = case

    type TestCaseParser<'a> =
        | Parser of (string -> TestCaseParser<'a>) 
        | ParsedTestCase of TestCase<'a>
    type NumberedTestCase<'a> = {case:'a; number:int}

    // Grab the number of test cases
    let getNumTestCases = System.Console.ReadLine >> int >> NumberOfCases

    let rec getNextTestCase caseParser =
        let caseParser' = System.Console.ReadLine() |> caseParser
        match caseParser' with
        | Parser parser -> getNextTestCase parser
        | ParsedTestCase case -> case

    let allTestCases caseParser =
        seq { for number in { 1 .. (getNumTestCases() |> numCasesToInt) }
            do yield { 
                case = getNextTestCase caseParser |> extractCase
                number = number
            } 
        }