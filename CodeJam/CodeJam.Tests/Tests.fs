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