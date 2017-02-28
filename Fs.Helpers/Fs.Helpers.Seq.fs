namespace Fs.Helpers.Seq

    module SeqExtensions =
        let rec (@@) fn value = seq { yield value; yield! fn @@ (fn value) }
