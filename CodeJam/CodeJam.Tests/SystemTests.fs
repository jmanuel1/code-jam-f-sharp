namespace CodeJam.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Text

[<TestClass>]
type ``When no arguments or invalid arguments are passed``() =
        
    [<TestMethod>]
    member this.``Display a usage message in stdout``() =
        let originalOut = Console.Out
        let possibilities = [
            [||]
            [|"--problem"|]
            [|"--problem"; "coin-jam"; "--problem"|]
            [|"--gibberish"|]
        ]
        possibilities |> List.iter (fun args ->
            let newOut = new IO.StringWriter()
            Console.SetOut(newOut)
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
                    UseShellExecute = false
                )
            let codejam = new Diagnostics.Process(StartInfo = codejamInfo)
            codejam.Start() |> ignore
            (* to avoid deadlocks: see Process.StandardOutput docs *)
            let output = new StringBuilder(codejam.StandardOutput.ReadToEnd())
            codejam.WaitForExit()
            output.Append(codejam.StandardOutput.ReadToEnd()) |> ignore
            Assert.IsFalse(String.IsNullOrWhiteSpace(string output), 
                "output was '" + string output + "'")
            newOut.Close()
            codejam.Close()
        )
        Console.SetOut(originalOut)

    [<TestMethod>]
    member this.``Display an error message in stderr``() =
        let originalError = Console.Error
        let possibilities = [
            [||]
            [|"--problem"|]
            [|"--problem"; "coin-jam"; "--problem"|]
            [|"--gibberish"|]
        ]
        possibilities |> List.iter (fun args ->
            let newError = new IO.StringWriter()
            Console.SetError(newError)
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
                    RedirectStandardError = true,
                    UseShellExecute = false
                )
            let codejam = new Diagnostics.Process(StartInfo = codejamInfo)
            codejam.Start() |> ignore
            (* to avoid deadlocks: see Process.StandardOutput docs *)
            let error = new StringBuilder(codejam.StandardError.ReadToEnd())
            codejam.WaitForExit()
            error.Append(codejam.StandardError.ReadToEnd()) |> ignore
            Assert.IsFalse(String.IsNullOrWhiteSpace(string error), 
                "error was '" + string error + "'")
            newError.Close()
            codejam.Close()
        )
        Console.SetError(originalError)