open System
open System.Diagnostics
open System

let run command =
  let timer = Stopwatch.StartNew()
  let procStartInfo = 
    ProcessStartInfo(
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false,
      CreateNoWindow = true,
      FileName = "cmd.exe",
      Arguments = sprintf "/C %s" command
    )
  use proc = new Process(StartInfo = procStartInfo)
  proc.OutputDataReceived |> Event.add (fun data -> printfn "%s" (data.Data))
  proc.ErrorDataReceived |> Event.add (fun data -> printfn "%s" (data.Data))
  proc.Start() |> ignore
  proc.BeginOutputReadLine()
  proc.BeginErrorReadLine()
  proc.WaitForExit()
  timer.Stop()
  printfn "%A" timer.Elapsed

[<EntryPoint>]
let main argv =
  Console.Write("> ")
  Console.ReadLine()
  |> run
  0 // return an integer exit code
