module Output
    type TestCaseOutput = {caseNumber:int; output:string}
    let outputString caseOut = "Case #" + (string caseOut.caseNumber) + ": " + caseOut.output
    let printCaseOutput = outputString >> printfn "%s"