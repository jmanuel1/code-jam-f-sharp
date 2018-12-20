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
        (* Use 16 and 50 to test handling of large numbers *)
        Console.SetIn(new IO.StringReader("1\n16 50"))
        let { Output.output = output; Output.caseNumber = number }::rest = 
            _2016_qu_coin_jam.solution |> Seq.toList

        Assert.AreEqual(1, number)
        Assert.IsTrue(List.isEmpty rest)

        let lines = output.TrimStart().Split('\n')
        let resultsArray = lines |> Array.map (fun line -> line.Split(' '))
        resultsArray |> Array.iter (fun result ->
            let coin = result.[0]
            let factorsForTesting = [
                (2, result.[1])
                (8, result.[7])
                (10, result.[9])
            ] 
            factorsForTesting |> List.iter (fun (radix, factor) ->
                (* Test only for bases 2, 8, 10 since custom implementation is 
                   needed for other bases, and that is already done in the
                   Coin Jam code *)
                let value = Convert.ToInt64(coin, radix)
                Assert.AreEqual(int64 0, value % int64 factor)
            ) 
        )

        Console.SetIn(in')

    [<TestMethod>]
    member this.``Outputs 9 factors for bases 2-10``() =
        (* This takes advantage of an internal detail: the use of Console.In *)
        let in' = Console.In
        Console.SetIn(new IO.StringReader("1\n6 3"))
        let { Output.output = output } = 
            _2016_qu_coin_jam.solution |> Seq.head
        let lines = output.TrimStart().Split('\n')
        let resultsArray = lines |> Array.map (fun line -> line.Split(' '))
        resultsArray |> Array.iter (fun result ->
            Assert.AreEqual(9, Array.length result.[1..])
        )
        Console.SetIn(in')

    [<TestMethod>]
    member this.``Mines the correct number of coins``() =
        (* This takes advantage of an internal detail: the use of Console.In *)
        let in' = Console.In
        Console.SetIn(new IO.StringReader("1\n6 3"))
        let { Output.output = output } = 
            _2016_qu_coin_jam.solution |> Seq.head
        let lines = output.TrimStart().Split('\n')
        Assert.AreEqual(3, Array.length lines)
        Console.SetIn(in')

    [<TestMethod>]
    member this.``'Case #x:' is put on its own line in the output``() =
        (* This takes advantage of an internal detail: the use of Console.In *)
        let in' = Console.In
        Console.SetIn(new IO.StringReader("1\n6 3"))
        let { Output.output = output } = 
            _2016_qu_coin_jam.solution |> Seq.head
        Assert.AreEqual('\n', output.[0])
        Console.SetIn(in')

[<TestClass>]
type ``'Rank and File' Tests`` () =

    [<TestMethod>]
    member this.``Can parse each test case``() =
        let unwrapParser (Input.Parser p) = p
        let rec chainParser p lines = 
            match lines with
            | [line] -> p line
            | line::rest -> chainParser (p line |> unwrapParser) rest
        let (Input.ParsedTestCase (Input.Case { _2016_1a_rank_and_file.n = n;
            _2016_1a_rank_and_file.lists = lists })) = 
            chainParser _2016_1a_rank_and_file.testCaseParser [
                "3"
                "1 2 3"
                "2 3 5"
                "3 5 6"
                "2 3 4"
                "1 2 3"
            ]
        Assert.AreEqual((3, [
            [1; 2; 3]
            [2; 3; 5]
            [3; 5; 6]
            [2; 3; 4]
            [1; 2; 3]
        ]), (n, lists))

    [<TestMethod>]
    member this.``Generates a complete grid from each test case with corect test case numbers``() =
        (* This takes advantage of an internal detail: the use of Console.In *)
        let in' = Console.In
        let inString = "1\n3\n1 2 3\n2 3 5\n3 5 6\n2 3 4\n1 2 3"
        let input = new IO.StringReader(inString)
        Console.SetIn(input)

        let { Output.output = output; Output.caseNumber = number }::rest = 
            _2016_1a_rank_and_file.solution |> Seq.toList

        Assert.AreEqual(1, number)
        Assert.IsTrue(List.isEmpty rest)

        Assert.AreEqual("3 4 6", output)

        Console.SetIn(in')