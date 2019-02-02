open System


let rec commandLoop () =
  Console.Write("> ")
  let command = Console.ReadLine()
  if command = "exit" then
    ()
  else
    match command |> Commands.map with
    | None -> ()
    | Some command -> Commands.exec command |> Seq.iter (fun file -> printfn "%s" file.Name)
    commandLoop ()

[<EntryPoint>]
let main argv =
  commandLoop ()
  0
