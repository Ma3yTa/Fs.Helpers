namespace Fs.Helpers.Option
    module OptionHelpers =
        let (|=) left right = defaultArg left right