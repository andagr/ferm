open System.IO

let ls path =
  Directory.EnumerateFileSystemEntries(path) 
  |> Seq.map FileInfo 
  |> Seq.map (fun fi -> fi.ToString())
