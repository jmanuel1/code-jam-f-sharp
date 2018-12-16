module _2016_qu_coin_jam
    open Input

    type TestData = {coinLength:int;numOfCoins:int}

    let testCaseParser (line:string) =
        let params = line.Split(' ')
        let coinLength = params.[0] |> int
        let numOfCoins = params.[1] |> int
        { coinLength = coinLength; numOfCoins = numOfCoins } |> Case |> ParsedTestCase