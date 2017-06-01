namespace Fs.Helpers.IO

open System
open System.IO

open Fs.Helpers.String.StringHelpers
open Fs.Helpers.Option.OptionHelpers
module IOHelpers =
    // TODO: This looks way overcomplicated, simplify.
    // Especially the PathCombiner
    let getTempDir = Path.GetTempPath |> Some
    let private validPathOrNone (p: string) =
        match p.IndexOfAny(Path.GetInvalidPathChars()) with
        | -1  -> p |> Some
        | _ -> None

    // TODO: Do some pre-checks, i.e. is the path valid, 2nd part is not absolute, etc.
    let private combinePath (p1: string) (p2: string) =
        match (validPathOrNone p1, validPathOrNone p2) with
        | (Some path1, Some path2) -> Path.Combine(path1, path2) |> Some
        | (Some _, None) -> Some p1
        | (None, Some _) -> Some p2
        | _ -> None


    type PathCombiner = PathCombiner with
        static member (?<-) (p1: string, _:PathCombiner, _) = fun (p2: string) -> combinePath p1 p2
        static member (?<-) (p1: string option, _:PathCombiner, _) =
            fun (p2: string option) ->
                match (p1, p2) with
                | (Some _, Some _) -> combinePath !!p1 !!p2
                | (Some _, None) -> validPathOrNone !!p1
                | _ -> None
        static member (?<-) (p1: string option, _:PathCombiner, _: string) =
            fun (p2: string) ->
                match (p1, p2) with
                | (Some _, s) when s |> isNullOrWhiteSpace -> validPathOrNone !!p1
                | (Some _, s) when s |> isNotNullOrWhiteSpace -> combinePath !!p1 p2
                | _ -> None
        static member (?<-) (p1: string, _:PathCombiner, _: string option) =
            fun (p2: string option) ->
                match (p1, p2) with
                | (s, Some _) when s |> isNullOrWhiteSpace -> validPathOrNone !!p2
                | (s, Some _) when s |> isNotNullOrWhiteSpace -> combinePath p1 !!p2
                | _ -> None


    let inline (+/) p1 p2 : ^T = (p1 ? (PathCombiner) <- Unchecked.defaultof< ^T> ) p2

    [<Sealed>]
    type FileWriter = FileWriter with
        static member ($) (FileWriter, str: string) = fun (filepath: string option, append: bool) ->
            match filepath with
            | Some fp ->
                use writer = new StreamWriter(fp, append)
                str |> writer.WriteLine
                true
            | None -> false
    let inline (.>) s f = (FileWriter $ s) (f, false)
    let inline (.>>) s f = (FileWriter $ s) (f, true)

    let fileExists = File.Exists
    let directoryExists path = Directory.Exists
    let inline readFile filepath =
        match filepath |> fileExists with
        | true ->
            try
                seq {
                    use sr = new StreamReader(filepath)
                    while not sr.EndOfStream do
                        yield sr.ReadLine ()
                } |> Some
            with
            | _ -> None
        | false -> None
