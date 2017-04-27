namespace Fs.Helpers.Lazy

module LazyHelpers =
    let inline (!) (a: Lazy<'a>) : 'a = a.Force()
