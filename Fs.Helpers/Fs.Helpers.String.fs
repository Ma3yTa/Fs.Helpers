namespace Fs.Helpers.String

open System

[<AutoOpen>]
module StringHelpers =
    let isNullOrEmpty = String.IsNullOrEmpty
    let isNotNullOrEmpty = not << isNullOrEmpty
    let isNullOrWhiteSpace = String.IsNullOrWhiteSpace
    let isNotNullOrWhiteSpace = not << isNullOrWhiteSpace