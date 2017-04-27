namespace Fs.Helpers.Monads


module MonadsHelpers =
    [<Sealed>]
    type Bind = Bind with
        static member (?<-) (s, _:Bind, _) = fun f -> Option.bind f s
        static member (?<-) (s, _:Bind, _) = fun f -> List.collect f s
        static member (?<-) (s, _:Bind, _) = fun f -> Array.collect f s
        static member (?<-) (s, _:Bind, _) = fun f -> Seq.collect f s
        static member (?<-) (s, _:Bind, _) = fun f -> async.Bind(s f)

    let inline (>>=) s f : ^T = (s ? (Bind) <- Unchecked.defaultof< ^T> ) f
    let inline (=<<) f s = s >>= f
