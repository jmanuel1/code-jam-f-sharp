namespace CodeJam.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Text

[<TestClass>]
type ``When invalid arguments are passed``() =
        
    member this.possibleInvalidArguments = [
        [|"--problem"|]
        [|"--problem"; "coin-jam"; "--problem"|]
        [|"--gibberish"|]
    ]

    member this.testOverPossibleInvalidArguments(test) =
        this.possibleInvalidArguments |> List.iter (fun args ->
            (* The following line causes the test runner to abort without
               telling me why. I think this happens because there is an
               `exit 1` in handleArgParsingError. *)
            //Program.main args |> ignore
            (* Since there is an exit, the test will have to drive the program 
               from the command line. *)
            (* Also, this test requires the CodeJam project to be built first. 
               And the working directory must be the configuration directory. 
               (That is, the default.)
               *)
            let codejamInfo =
                new Diagnostics.ProcessStartInfo(
                    WindowStyle = Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "..\\..\\..\\CodeJam\\bin\\Debug\\CodeJam.exe",
                    Arguments = String.Join(" ", args),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                )
            let codejam = new Diagnostics.Process(StartInfo = codejamInfo)
            codejam.Start() |> ignore
            (* to avoid deadlocks: see Process.StandardOutput docs *)
            let output = new StringBuilder(codejam.StandardOutput.ReadToEnd())
            let error = new StringBuilder(codejam.StandardError.ReadToEnd())
            codejam.WaitForExit()
            output.Append(codejam.StandardOutput.ReadToEnd()) |> ignore
            error.Append(codejam.StandardError.ReadToEnd()) |> ignore
            test codejam (string output) (string error)
            codejam.Close()
        )

    [<TestMethod>]
    member this.``Display a usage message in stdout``() =
        this.testOverPossibleInvalidArguments (fun _ output _ ->
            Assert.IsFalse(String.IsNullOrWhiteSpace(output), 
                "output was '" + string output + "'")
        )

    [<TestMethod>]
    member this.``Display an error message in stderr``() =
        this.testOverPossibleInvalidArguments (fun _ _ error ->
            Assert.IsFalse(String.IsNullOrWhiteSpace(error), 
                "error was '" + string error + "'")
        )

    [<TestMethod>]
    member this.``Return an exit code of one``() =
        this.testOverPossibleInvalidArguments (fun codejam _ _ ->
            let exitCode = codejam.ExitCode
            Assert.AreEqual(1, exitCode)
        )

    [<TestMethod>]
    member this.``The error message must start with 'Error: EARGS - '``() =
        this.testOverPossibleInvalidArguments (fun _ _ error ->
            Assert.IsTrue(error.StartsWith("Error: EARGS - "))
        )

    [<TestMethod>]
    member this.``The error message must suggest how to fix the error``() =
        this.testOverPossibleInvalidArguments (fun _ _ error ->
            Assert.IsTrue(error.Contains("Fix with: "))
        )

    [<TestMethod>]
    member this.``The error message must contain a link to further info``() =
        this.testOverPossibleInvalidArguments (fun _ _ error ->
            Assert.IsTrue(error.Contains("https://"))
        )

[<TestClass>]
type ``When given '--problem rank-and-file' as arguments``() =

    [<TestMethod>]
    member this.``Wait for input from stdin without output``() =
        (* This test requires the CodeJam project to be built first. 
           And the working directory must be the configuration directory. 
           (That is, the default.) *)
        let codejamInfo = 
            new Diagnostics.ProcessStartInfo(
                WindowStyle = Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "..\\..\\..\\CodeJam\\bin\\Debug\\CodeJam.exe",
                Arguments = "--problem rank-and-file",
                RedirectStandardOutput = true,
                UseShellExecute = false
            )
        let codejam = new Diagnostics.Process(StartInfo = codejamInfo)
        codejam.Start() |> ignore
        codejam.WaitForExit(1000) |> ignore
        codejam.Kill()
        let output = new StringBuilder(codejam.StandardOutput.ReadToEnd())
        Assert.IsTrue(String.IsNullOrWhiteSpace(string output), 
            "output was '" + string output + "'")
        
        codejam.Close()

    [<TestMethod>]
    member this.``Write output to stdout based on stdin``() =
        let originalIn = Console.In
        let originalOut = Console.Out
        let newIn =
            new IO.StringReader("1\n3\n1 2 3\n2 3 5\n3 5 6\n2 3 4\n1 2 3")
        let newOut = new IO.StringWriter()
        Console.SetIn(newIn)
        Console.SetOut(newOut)
        Program.main [|"--problem"; "rank-and-file"|] |> ignore
        let output = string newOut
        Assert.IsTrue(output.Contains("Case #"))
        Console.SetIn(originalIn)
        Console.SetOut(originalOut)

[<TestClass>]
type ``Upon success``()=

    [<TestMethod>]
    member this.``Return an exit code of zero``() =
        let originalIn = Console.In
        let originalOut = Console.Out
        let newIn =
            new IO.StringReader("1\n3\n1 2 3\n2 3 5\n3 5 6\n2 3 4\n1 2 3")
        let newOut = new IO.StringWriter()
        Console.SetIn(newIn)
        Console.SetOut(newOut)
        let exitCode = Program.main [|"--problem"; "rank-and-file"|]
        Assert.AreEqual(0, exitCode)
        Console.SetIn(originalIn)
        Console.SetOut(originalOut)

[<TestClass>]
type ``Gives a help message``()=

    member this.helpTest(args) =
        let originalOut = Console.Out
        let newOut = new IO.StringWriter()
        Console.SetOut(newOut)
        Program.main args |> ignore
        Assert.IsFalse(string newOut |> String.IsNullOrWhiteSpace)
        Console.SetOut(originalOut)

    [<TestMethod>]
    member this.``When passed no arguments``() =
        this.helpTest([||])

    [<TestMethod>]
    member this.``When passed '--help' as an argument``() =
        this.helpTest([|"--help"|])

    [<TestMethod>]
    member this.``When passed 'help' as an argument``() =
        this.helpTest([|"help"|])

    [<TestMethod>]
    member this.``When passed '-h' as an argument``() =
        this.helpTest([|"-h"|])