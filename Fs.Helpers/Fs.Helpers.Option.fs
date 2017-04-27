namespace Fs.Helpers.Option

module OptionHelpers =
    let inline (|=) left right = defaultArg left right
    let inline (!) (opt: 'a option) : 'a = Option.get opt