open System

let writer output =
  output |> Seq.iter (printfn "%s")

let rec commandLoop () =
  Console.Write("> ")
  let command = Console.ReadLine()
  if command = "exit" then ()
  else
    command |> Fsi.eval |> printfn "%A"
    commandLoop ()

[<EntryPoint>]
let main argv =
  commandLoop ()
  0
