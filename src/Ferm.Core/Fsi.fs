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
  let argv = [|"fsi.exe"; "--noninteractive"|]
  let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
  FsiEvaluationSession.Create(fsiConfig, argv, inputStream, outputStream, errorsStream), output, errors

let private fsiSession, output, errors = createFsi

let eval code =
  try
    match fsiSession.EvalExpression(code) with // Todo: Eval interactive / script
    | Some value -> value.ReflectionValue
    | None -> "No result" :> obj
  with
    | _ -> 
      let error = errors.ToString() :> obj
      errors.Clear() |> ignore
      error
