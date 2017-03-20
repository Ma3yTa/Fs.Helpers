namespace Fs.Helpers.Http

module HttpExtensions =
    open System
    open System.Net.Http
    open System.IO

    open FSharp.Core
    open FSharp.Reflection

    let private tryParseUri (uri: string) : Uri option =
        match Uri.TryCreate(uri, UriKind.Absolute) with
        | (true, url) -> Some url
        | _ -> None

    type HttpHeader = string * string

    type HttpContext = {
        method: HttpMethod option;
        uri: Uri option;
        headers: (HttpHeader list) option;
        body: byte array option;
        timeout: int option;
        keepalive: bool option;
    }

    type HttpResponse = {
        statusCode: int option;
        contentLength: int64 option;
        uri: Uri option;
        headers: HttpHeader list option;
        body: Stream option
    }

    // TODO:
    // Not sure that would be the proper way of "verifying" the context
    // Maybe it's worth it to do some more "monadic" way
    let private validateHttpContext(ctx: HttpContext) : HttpContext option =
        match ctx with
        | ctx when ctx.method = None || ctx.uri = None  -> None
        | _ -> Some ctx

    let private performHttpRequest (ctx: HttpContext) : HttpResponse option =
        use client = new HttpClient ()
        use message = new HttpRequestMessage ()
        message.Method <- Option.get ctx.method
        message.RequestUri <- Option.get ctx.uri
        match ctx.headers with
        | Some headers -> headers |> Seq.iter message.Headers.Add
        | None -> ()
        None

    type HttpBuilder () =
        member this.Zero() = {
            method = None
            uri = None
            headers = None
            body = None
            timeout = None
            keepalive = None
        }
        member this.Yield(_) = this.Zero()
        (* HTTP related Methods *)
        [<CustomOperation("timeout", MaintainsVariableSpace=true)>]
        member this.Timeout(ctx: HttpContext, timeout: int) =
            { ctx with timeout = Some timeout}
        [<CustomOperation("keepalive", MaintainsVariableSpace=true)>]
        member this.Keepalive(ctx: HttpContext, keepalive: bool) =
            { ctx with keepalive = Some keepalive}
        [<CustomOperation("GET", MaintainsVariableSpace=true)>]
        member this.GET(ctx: HttpContext, uri: string, headers: (string * string) list option) =
            { ctx with method = Some HttpMethod.Get; uri = uri |> tryParseUri; headers = headers }

        member __.Run(ctx: HttpContext) =
            match ctx |> validateHttpContext with
            | Some ctx -> ctx |> performHttpRequest
            | None -> None

    let http = HttpBuilder()

    let response = http {
            timeout 10_000  // 10 seconds
            keepalive false
            GET "https://microsoft.com/" None
    }
    // response |> printfn
