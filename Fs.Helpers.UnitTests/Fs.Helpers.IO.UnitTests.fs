module Fs.Helpers.IO.UnitTests

open NUnit.Framework
open FsUnitTyped
open FsUnit

open Fs.Helpers.IO.IOHelpers

[<Test>]
let ``Test basic file write`` () =
    "HAI" .> "C:\somefile.temp"