module _2016_qu_coin_jam
    open Input
    open Output
    open System.Linq
    open System
    open System.Numerics

    type TestData = {coinLength:int; numOfCoins:int}

    let testCaseParser (line:string) =
        let testParams = line.Split(' ')
        let coinLength = testParams.[0] |> int
        let numOfCoins = testParams.[1] |> int
        { 
            coinLength = coinLength
            numOfCoins = numOfCoins 
        } |> Case |> ParsedTestCase
    
    let parseWithRadix coin (radix:int) =
        let radix' = bigint radix
        let folder = fun (acc, place) digit ->
            (acc + (string digit |> bigint.Parse)*(pown radix' place), 
                place - 1)
        let (value, _) = 
            List.fold folder (0I, String.length coin - 1) <| List.ofSeq coin
        value

    let validate coin =
        let findFactorsInBases = Seq.fold (fun (factors, isPrime) radix ->
            if isPrime then (factors, true)
            else
                let num = parseWithRadix coin radix
                let factorSearch() =
                    let primishFilter = 
                        Seq.filter (fun (i:int) -> 
                            let i' = bigint i
                            i'*i' <= num && (i' + 1I) % 6I = 0I)
                    let factorsFolder = (fun (factors, isPrime) (i:int) ->
                        let i' = bigint i
                        if not isPrime then (factors, false)
                        else
                            if num % i' = 0I then
                                (List.append factors [i'], false)
                            elif num % (i' + 2I) = 0I then
                                (List.append factors [i' + 2I], false)
                            else (factors, isPrime))
                    let findFactors = Seq.fold factorsFolder (factors, true)
                    Enumerable.Range(5, 299700) |> primishFilter |> findFactors
                (* prime test taken from 
                   https://en.wikipedia.org/wiki/Primality_test *)
                if num <= 3I then
                    (factors, num > 1I || isPrime)
                elif num % 2I = 0I then
                    (List.append factors [2I], isPrime)
                elif num % 3I = 0I then
                    (List.append factors [3I], isPrime)
                else factorSearch()) ([], false)
        (* Range takes parameters start, count: count is number of items 
           including start *)
        let primeTest = Enumerable.Range(2, 9) |> findFactorsInBases
        match primeTest with
        | (_, true) -> None
        | (factors, _) -> Some factors

    let solution =
        seq {
            for case in allTestCases testCaseParser do
                let { coinLength = coinLength; numOfCoins = numOfCoins } = 
                    case.case
                let findAndOutputCoinFromVar = 
                    let possiblyOutputCoin coin coinNumber output =
                        match validate coin with
                        | Some factors -> 
                            let factorsString = String.Join(" ", factors)
                            let newOutput = 
                                sprintf "%s\n%s %s" output coin factorsString
                            (coinNumber + 1, newOutput)
                        | None -> (coinNumber, output)
                    fun (coinNumber, output) (var:int) ->
                        if coinNumber > numOfCoins then
                            (coinNumber, output) (* Do nothing. *)
                        else
                            let strVar = Convert.ToString(var, 2)
                            let numberOfPadZeros = 
                                coinLength - 2 - String.length strVar
                            let zeroPaddedStrVar = 
                                String.replicate numberOfPadZeros "0" + strVar
                            let coin = "1" + zeroPaddedStrVar + "1" 
                            possiblyOutputCoin coin coinNumber output
                let findCoinsFromVars = 
                    Seq.fold findAndOutputCoinFromVar (1, "")
                let varRange = Enumerable.Range(0, pown 2 (coinLength - 2))
                let output = varRange |> findCoinsFromVars |> snd
                yield { caseNumber = case.number; output = output } }