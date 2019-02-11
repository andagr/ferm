open System
open System.IO
open System.Text.RegularExpressions

module Command =
  type private Marker = interface end
  let t = typeof<Marker>.DeclaringType

  type CommandAttribute() = inherit Attribute()

  let definedCommands =
    t.GetMembers()
    |> Array.filter(fun mi -> mi.CustomAttributes |> Seq.exists (fun a -> a.AttributeType = typeof<CommandAttribute>))

  let mapCommands (command : string) =
    let addOrQuoteArgs cmd =
      if definedCommands |> Array.exists (fun dc -> dc.Name = cmd) then
        sprintf "Command.%s \"\"" cmd
      else
        let parts = Regex.Split(cmd, @"\s+")
        let baseCmd = parts |> Array.head
        let args = parts |> Array.tail |> (fun a -> String.Join(" ", a))
        if String.length args > 1 && definedCommands |> Array.exists (fun dc -> dc.Name = baseCmd) then
          sprintf "Command.%s \"%s\"" baseCmd args
        else
          cmd
    let commands = 
      command.Split([|"|>"|], StringSplitOptions.RemoveEmptyEntries) 
      |> Array.map (fun c -> c.Trim())
    commands
    |> Array.map addOrQuoteArgs
    |> (fun c -> String.Join(" |> ", c))



  [<Command>]
  let ls path =
    Directory.EnumerateFileSystemEntries(path) 
    |> Seq.map FileInfo 
    |> Seq.map (fun fi -> fi.ToString())

let ls = Command.ls "."