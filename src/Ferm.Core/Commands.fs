module Commands

open System.Text.RegularExpressions
open System.IO

type Write = string seq -> unit

let ls args =
  Directory.EnumerateFileSystemEntries(Seq.head args) 
  |> Seq.map FileInfo 
  |> Seq.map (fun fi -> fi.ToString())
  |> ignore

let map input =
  let inputParts = 
    Regex.Split(input, @"\s+") // Todo: Handle quoted strings with whitespace
    |> Array.filter (fun p -> String.length p > 0)
    |> List.ofArray
  match inputParts with
  | ["ls"] -> ls ["."]
  | "ls"::args -> ls args
  | _ -> ()

let exec (write : Write) command =
  match command with
  | None -> ()