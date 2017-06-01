module Fs.Helpers.Monads.UnitTests

open NUnit.Framework
open FsUnitTyped
open FsUnit

open Fs.Helpers.Monads.MonadsHelpers
open Fs.Helpers.Option.OptionHelpers

[<Test>]
let ``Test basic file write into temp directory`` () =
    let divideBy b t =
        match b with
        | 0 -> None
        | _ -> t/b |> Some

    let ``Some result`` = 10 |> divideBy 1 >>= divideBy 2 >>= divideBy 5
    ``Some result`` |> Option.isSome |> should be True
    !!``Some result`` |> should equal 1

    let ``None result`` = 10 |> divideBy 1 >>= divideBy 0 >>= divideBy 5
    ``None result`` |> Option.isNone |> should be True

