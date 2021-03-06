#r "netstandard"
#r "../packages/FSharp.Compiler.Service/lib/netstandard2.0/FSharp.Compiler.Service.dll"
#r "../packages/System.Data.SqlClient/lib/netstandard2.0/System.Data.SqlClient.dll"

open Microsoft.FSharp.Compiler.SourceCodeServices

let input =
  """
  let x = "hej"
  x.
  """
let inputLines = input.Split('\n')

let file = "Text.fsx"

let checker = FSharpChecker.Create()
let projOptions, errors = checker.GetProjectOptionsFromScript(file, input) |> Async.RunSynchronously
let parsingOptions, _errors = checker.GetParsingOptionsFromProjectOptions(projOptions)
let parseResults, checkFileAnswer = checker.ParseAndCheckFileInProject(file, 0, input, projOptions) |> Async.RunSynchronously
let checkFileResults =
  match checkFileAnswer with
  | FSharpCheckFileAnswer.Succeeded(res) -> res
  | res -> failwithf "Parsing did not finish... (%A)" res
let identToken = FSharpTokenTag.IDENT
let tip = checkFileResults.GetToolTipText(4, 7, inputLines.[1], ["foo"], identToken) |> Async.RunSynchronously
printfn "%A" tip

