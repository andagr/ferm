module Commands

open System.Text.RegularExpressions
open System.IO

type Write = string seq -> unit

type Core =
  | Ls of string seq

let map input =
  let inputParts = 
    Regex.Split(input, @"\s+") // Todo: Handle quoted strings with whitespace
    |> Array.filter (fun p -> String.length p > 0)
    |> List.ofArray
  match inputParts with
  | ["ls"] -> Some (Ls ["."])
  | "ls"::args -> Some (Ls args)
  | _ -> None

let exec (write : Write) command =
  match command with
  | Some (Ls args) -> 
      Directory.EnumerateFileSystemEntries(Seq.head args) 
      |> Seq.map FileInfo 
      |> Seq.map (fun fi -> fi.ToString())
      |> write
  | None -> ()