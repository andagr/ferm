module Fsi

open Microsoft.FSharp.Compiler.SourceCodeServices
open Microsoft.FSharp.Compiler.Interactive.Shell

open System.IO
open System.Text

let private createFsi =
  let output = new StringBuilder()
  let errors = new StringBuilder()
  let inputStream = new StringReader("")
  let outputStream = new StringWriter(output)
  let errorsStream = new StringWriter(errors)
  let argv = [|"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community\\Common7\\IDE\\CommonExtensions\\Microsoft\\FSharp\\fsi.exe"; "--noninteractive"|]
  let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
  FsiEvaluationSession.Create(fsiConfig, argv, inputStream, outputStream, errorsStream)

let private fsiSession = createFsi

let eval code =
  match fsiSession.EvalExpression(code) with
  | Some value -> value.ReflectionValue
  | None -> "No result" :> obj
