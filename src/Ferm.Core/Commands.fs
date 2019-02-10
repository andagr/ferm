module Commands

open System.IO

let load writer =
  Fsi.evalInteractive (File.ReadAllText(@"..\Ferm.Core\commands.fsx")) |> writer

let exec writer command =
  match Fsi.evalInteractive command with
  | Result.Ok _ -> ()
  | e -> writer e