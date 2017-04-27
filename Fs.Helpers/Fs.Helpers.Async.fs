namespace Fs.Helpers.Async

module AsyncHelpers =
    open Fs.Helpers.Option.OptionHelpers
    let sync a = a |> Async.RunSynchronously
