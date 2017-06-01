module Fs.Helpers.IO.UnitTests

open NUnit.Framework
open FsUnitTyped
open FsUnit

open Fs.Helpers.IO.IOHelpers
open Fs.Helpers.Option.OptionHelpers

[<Test>]
let ``Test basic file write into temp directory`` () =
    let tempDir = getTempDir
    tempDir |> Option.isSome |> should equal true

    let tempFilePath = !!tempDir +/ "TempFileName.txt"
    tempFilePath |> Option.isSome |> should equal true


    let result = "TEST" .> tempFilePath
    result |> should equal true

    let fileContents = readFile !!tempFilePath
    fileContents |> Option.isSome |> should equal true

