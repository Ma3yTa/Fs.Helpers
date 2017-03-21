module Fs.Helpers.UnitTests

open NUnit.Framework
open FsUnit

open Fs.Helpers.Http.HttpExtensions

[<Test>]
let ``Test basic http request without mocking`` () = http {
    GET "https://github.com/" None
}
