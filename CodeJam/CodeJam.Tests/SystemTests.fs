namespace CodeJam.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

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
        List.iter (fun args ->
            let newOut = new IO.StringWriter()
            Console.SetOut(newOut)
            (* The following line causes the test runner to abort without
               telling me why. I think this happens because main has the
               EntryPoint attribute. *)
            //Program.main args |> ignore
            Assert.IsFalse(String.IsNullOrWhiteSpace(newOut.ToString()))
            newOut.Close()
        ) possibilities
        Console.SetOut(originalOut)