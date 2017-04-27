namespace Fs.Helpers.IO

open System.IO

module IOHelpers =
    [<Sealed>]
    type FileWriter = FileWriter with
        static member ($) (FileWriter, str :string) = fun (filepath: string) ->
            use writer = new StreamWriter(filepath)
            str |> writer.WriteLine
    let inline (.>) s f = (FileWriter $ s) f