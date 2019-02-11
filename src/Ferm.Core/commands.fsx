open System
open System.IO

module Commands =
  type private Marker = interface end
  let t = typeof<Marker>.DeclaringType

  type CommandAttribute() = inherit Attribute()

  let definedCommands =
    t.GetMembers()
    |> Array.filter(fun mi -> mi.CustomAttributes |> Seq.exists (fun a -> a.AttributeType = typeof<CommandAttribute>))

  let addDefaultArgs (command : string) =
    let addArgs cmd =
      if definedCommands |> Array.exists (fun dc -> dc.Name = cmd) then
        sprintf "%s \"\"" cmd
      else cmd
    let commands = 
      command.Split([|"|>"|], StringSplitOptions.RemoveEmptyEntries) 
      |> Array.map (fun c -> c.Trim())
    commands
    |> Array.map addArgs
    |> (fun c -> String.Join(" |> ", c))



  [<Command>]
  let ls path =
    Directory.EnumerateFileSystemEntries(path) 
    |> Seq.map FileInfo 
    |> Seq.map (fun fi -> fi.ToString())

Commands.addDefaultArgs "ls |> dir"