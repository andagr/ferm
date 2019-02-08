open System.IO

let ls args =
  Directory.EnumerateFileSystemEntries(Seq.head args) 
  |> Seq.map FileInfo 
  |> Seq.map (fun fi -> fi.ToString())