module _2016_qu_coin_jam
    open Input

    type TestData = {coinLength:int;numOfCoins:int}

    let testCaseParser (line:string) =
        let testParams = line.Split(' ')
        let coinLength = testParams.[0] |> int
        let numOfCoins = testParams.[1] |> int
        { coinLength = coinLength; numOfCoins = numOfCoins } |> Case |> ParsedTestCase