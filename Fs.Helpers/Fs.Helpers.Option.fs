namespace Fs.Helpers.Option

module OptionHelpers =
    let inline (|=) left right = defaultArg left right
