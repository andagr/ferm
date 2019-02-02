module Commands
open System.Text.RegularExpressions
open System.IO
open System.IO

type Core =
  | Ls of string seq

let map input =
  let inputParts = 
    Regex.Split(input, @"\s+") // Todo: Handle quoted strings with whitespace
    |> Array.filter (fun p -> String.length p > 0)
  match inputParts |> Array.head with
  | "ls" -> Some (Ls (Array.tail inputParts))
  | _ -> None

let exec command =
  match command with
  | Ls args -> Directory.EnumerateFileSystemEntries(Seq.head args) |> Seq.map FileInfo