module Fsi

open Microsoft.FSharp.Compiler.SourceCodeServices
open Microsoft.FSharp.Compiler.Interactive.Shell

open System.IO
open System.Text
open System.Text.RegularExpressions

let private createFsi =
  let output = StringBuilder()
  let errors = StringBuilder()
  let inputStream = new StringReader("")
  let outputStream = new StringWriter(output)
  let errorsStream = new StringWriter(errors)
  let argv = [|"fsi.exe"; "--noninteractive"|]
  let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
  FsiEvaluationSession.Create(fsiConfig, argv, inputStream, outputStream, errorsStream), output, errors

let private fsiSession, outSb, errors = createFsi

let evalInteractive code =
  let result, errorInfo = fsiSession.EvalInteractionNonThrowing(code)
  let warnings = 
    errorInfo 
    |> Array.map (fun w -> sprintf "Warning %s at %d, %d" w.Message w.StartLineAlternate w.StartColumn)
    |> (fun ws -> System.String.Join("\n", ws))
  match result with
  | Choice1Of2 _ -> 
    let output =  outSb.ToString()
    outSb.Clear() |> ignore
    Result.Ok (sprintf "%s\n\n%s" warnings output)
  | Choice2Of2 ex -> Result.Error (sprintf "%s\n\n%s\n" warnings ex.Message)

let parseIt (output : string) =
  // val it : string = "Command.ls """
  let itVal = Regex.Match((output.Trim()), "[^=]+ = (.+)$").Groups.[1].Value
  if itVal.StartsWith("\"") then itVal.Substring(1, String.length itVal - 2)
  else itVal

let init writer =
  match evalInteractive (File.ReadAllText(@"../Ferm.Core/Command.fsx")) with
  | Ok _ -> ()
  | Error e -> writer (Result.Error e)

let mapCommands writer command =
  match evalInteractive (sprintf "Command.mapCommands \"%s\"" command) with
  | Ok output -> output |> parseIt |> evalInteractive |> writer
  | Error e -> writer (Result.Error e)
