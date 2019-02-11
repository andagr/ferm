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
    command |> Fsi.exec writer
    commandLoop ()

[<EntryPoint>]
let main argv =
  Fsi.init writer
  commandLoop ()
  0
