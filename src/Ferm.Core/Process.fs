module Process

open System.Diagnostics
open System.Collections.Generic

let run program args =
  let stdoutLines = new List<string>();
  let stderrLines = new List<string>();
  let procStartInfo = 
    ProcessStartInfo(
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      RedirectStandardInput = true,
      UseShellExecute = false,
      CreateNoWindow = true,
      FileName = program,
      Arguments = args
    )
  use proc = new Process(StartInfo = procStartInfo)
  proc.OutputDataReceived |> Event.add (fun stdout -> stdoutLines.Add(stdout.Data))
  proc.ErrorDataReceived |> Event.add (fun stderr -> stderrLines.Add(stderr.Data))
  proc.Start() |> ignore
  proc.BeginOutputReadLine()
  proc.BeginErrorReadLine()
  proc.WaitForExit()
  (stdoutLines |> List.ofSeq, stderrLines |> List.ofSeq)