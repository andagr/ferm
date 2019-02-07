open System

let writer output =
  output |> Seq.iter (printfn "%s")

let rec commandLoop () =
  Console.Write("> ")
  let command = Console.ReadLine()
  if command = "exit" then ()
  else
    command |> Commands.map |> Commands.exec writer
    commandLoop ()

[<EntryPoint>]
let main argv =
  commandLoop ()
  0
