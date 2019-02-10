open System

let writer result =
  match result with
  | Result.Ok output -> printfn "%s" output
  | Result.Error (error : string) -> stderr.Write(error)

let rec commandLoop () =
  Console.Write("> ")
  let command = Console.ReadLine()
  if command = "exit" then ()
  else
    command |> Commands.exec writer
    commandLoop ()

[<EntryPoint>]
let main argv =
  Commands.load writer
  commandLoop ()
  0
