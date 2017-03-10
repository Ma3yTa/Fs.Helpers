namespace Fs.Helpers.Option
    module OptionHelpers =
        open FSharp.Core.Operators
        let (|=) left right = defaultArg left right