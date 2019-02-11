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

let evalInteractive code =
  let result, errorInfo = fsiSession.EvalInteractionNonThrowing(code)
  let warnings = 
    errorInfo 
    |> Array.map (fun w -> sprintf "Warning %s at %d, %d" w.Message w.StartLineAlternate w.StartColumn)
    |> (fun ws -> System.String.Join("\n", ws))
  match result with
  | Choice1Of2 () -> 
    let out =  output.ToString()
    output.Clear() |> ignore
    Result.Ok (sprintf "%s\n\n%s" warnings out)
  | Choice2Of2 ex -> Result.Error (sprintf "%s\n\n%s" warnings ex.Message)

let init writer =
  evalInteractive (File.ReadAllText(@"../Ferm.Core/Command.fsx")) |> writer

let mapCommands writer command =
  match evalInteractive ("Command.mapCommands " + command) with
  | Result.Ok c -> printfn ">>> %s <<<" c
  | e -> writer e
