namespace CodeJam.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type ``'The Last Word' Tests`` () =

    [<TestMethod>]
    member this.``Each line is a single test case containing the initial string``() =
        let (Input.ParsedTestCase (Input.Case { _2016_1a_the_last_word.str = actual })) = 
            _2016_1a_the_last_word.testCaseParser "TEST"
        Assert.AreEqual("TEST", actual)

    [<TestMethod>]
    member this.``Solves and numbers every sample test case``() =
        (* This takes advantage of an internal detail: the use of Console.In *)
        let in' = Console.In
        Console.SetIn(new IO.StringReader(
            "5\n" +
            "CAB\n" +
            "JAM\n" +
            "CODE\n" +
            "ABAAB\n" +
            "CABCBBABC\n"
        ))
        let actual = _2016_1a_the_last_word.solution |> Seq.toList
        let actualOutput = 
            List.map (fun { Output.output = actual } -> actual) actual
        let actualNumbers = 
            List.map (fun { Output.caseNumber = actual } -> actual) actual
        Assert.AreEqual(["CAB"; "MJA"; "OCDE"; "BBAAA"; "CCCABBBAB"], 
            List.ofSeq actualOutput, "last word output is wrong")
        Assert.AreEqual([1; 2; 3; 4; 5], List.ofSeq actualNumbers, 
            "test output numbered incorrectly")
        Console.SetIn(in')

[<TestClass>]
type ``'Coin Jam' Tests`` () =

    [<TestMethod>]
    member this.``Each test case line is parsed``() =
        let (Input.ParsedTestCase (Input.Case { _2016_qu_coin_jam.coinLength = actualLen; _2016_qu_coin_jam.numOfCoins = actualNum })) = 
            _2016_qu_coin_jam.testCaseParser "6 3"
        Assert.AreEqual((6, 3), (actualLen, actualNum))

    [<TestMethod>]
    member this.``Generates valid jamcoins with correct test case numbers``() =
        (* This takes advantage of an internal detail: the use of Console.In *)
        let in' = Console.In
        Console.SetIn(new IO.StringReader("1\n6 3"))
        let { Output.output = output; Output.caseNumber = number }::rest = 
            _2016_qu_coin_jam.solution |> Seq.toList

        Assert.AreEqual(1, number)
        Assert.IsTrue(List.isEmpty rest)

        let lines = output.Split('\n')
        let resultsArray = lines |> Array.map (fun line -> line.Split(' '))
        resultsArray |> Array.iter (fun result ->
            let coin = result.[0]
            let factors = result.[1..]
            Array.iteri (fun i factor ->
                let radix = i + 2
                let value = this.parseWithRadix(coin, radix)
                Assert.AreEqual(0, value % int factor)
            ) factors
        )
        Console.SetIn(in')

    member private this.parseWithRadix(coin, radix) =
        let folder = fun (acc, place) digit ->
            (acc + (int digit)*(pown radix place), place - 1)
        let (value, _) = 
            List.fold folder (0, String.length coin - 1) <| List.ofSeq coin
        value