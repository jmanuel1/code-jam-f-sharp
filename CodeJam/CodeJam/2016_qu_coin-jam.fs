module _2016_qu_coin_jam
    open Input
    open Output
    open System.Linq
    open System

    type TestData = {coinLength:int; numOfCoins:int}

    let testCaseParser (line:string) =
        let testParams = line.Split(' ')
        let coinLength = testParams.[0] |> int
        let numOfCoins = testParams.[1] |> int
        { 
            coinLength = coinLength
            numOfCoins = numOfCoins 
        } |> Case |> ParsedTestCase
    
    let parseWithRadix coin radix =
        let folder = fun (acc, place) digit ->
            (acc + (int digit)*(pown radix place), place - 1)
        let (value, _) = 
            List.fold folder (0, String.length coin - 1) <| List.ofSeq coin
        value

    let validate coin =
        let findFactorsInBases = Seq.fold (fun (factors, isPrime) radix ->
            if isPrime then (factors, true)
            else
                let num = parseWithRadix coin radix
                let factorSearch() =
                    let primishFilter = 
                        Seq.filter (fun i -> i*i <= num && (i + 1) % 6 = 0)
                    let findFactors = Seq.fold (fun (factors, isPrime) i ->
                        if not isPrime then (factors, false)
                        else
                            if num % i = 0 then
                                (List.append factors [i], false)
                            elif num % (i + 2) = 0 then
                                (List.append factors [i + 2], false)
                            else (factors, isPrime)) (factors, true)
                    Enumerable.Range(5, 299700) |> primishFilter |> findFactors
                (* prime test taken from 
                   https://en.wikipedia.org/wiki/Primality_test *)
                if num <= 1 then
                    (factors, isPrime)
                elif num <= 3 then
                    (factors, true)
                elif num % 2 = 0 then
                    (List.append factors [2], isPrime)
                elif num % 3 = 0 then
                    (List.append factors [3], isPrime)
                else factorSearch()) ([], false)
        let primeTest = Enumerable.Range(2, 11) |> findFactorsInBases
        match primeTest with
        | (_, true) -> None
        | (factors, _) -> Some factors

    let solution =
        seq {
            for case in allTestCases testCaseParser do
                let { coinLength = coinLength; numOfCoins = numOfCoins } = 
                    case.case
                let findAndOutputCoinFromVar = fun (coinNumber, output) (var:int) ->
                    let possiblyOutputCoin coin =
                        match validate coin with
                        | Some factors -> 
                            let factorsString = String.Join(" ", factors)
                            let newOutput = 
                                sprintf "%s\n%s %s" output coin factorsString
                            (coinNumber + 1, newOutput)
                        | None -> (coinNumber, output)
                    if coinNumber > numOfCoins then
                        (coinNumber, output) (* Do nothing. *)
                    else
                        let strVar = Convert.ToString(var, 2)
                        let numberOfPadZeros = 
                            coinLength - 2 - String.length strVar
                        let zeroPaddedStrVar = 
                            String.replicate numberOfPadZeros "0" + strVar
                        let coin = "1" + zeroPaddedStrVar + "1" 
                        possiblyOutputCoin coin
                let findCoinsFromVars = 
                    Seq.fold findAndOutputCoinFromVar (1, "")
                let varRange = Enumerable.Range(0, pown 2 (coinLength - 2))
                let output = varRange |> findCoinsFromVars |> snd
                yield { caseNumber = case.number; output = output } }