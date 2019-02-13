open System.Text.RegularExpressions

let output = 
  "\n" +
  "\n" +
  "val it : string = \"Command.ls \"\"\"" +
  "\n"
let trimmedOutput = output.Trim()
let parser = Regex("[^=]+ = (.+)$")
let matcher = parser.Match(trimmedOutput)
let itVal = matcher.Groups.[1].Value
if itVal.StartsWith("\"") then itVal.Substring(1, String.length itVal - 2)
else itVal