module Fs.Helpers.UnitTests

open NUnit.Framework
open FsUnitTyped
open FsUnit

open Fs.Helpers.Http.HttpHelpers

[<Test>]
let ``Test basic http request without mocking`` () =
    let request = http {
        timeout 100000  // 100 seconds
        keepalive false
        GET "https://microsoft.com/" None
    }

    let result = request |> getResponse
    result |> shouldNotEqual None