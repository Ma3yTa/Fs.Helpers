namespace Fs.Helpers.Http

module HttpExtensions =
    open System
    open System.Net
    open System.IO

    open FSharp.Core
    open FSharp.Reflection

    [<Literal>] let private CRLF = "\r\n"
    // From http://www.fssnip.net/9l/title/toString-and-fromString-for-discriminated-unions
    let private unionToString (x: 'a) =
        match FSharpValue.GetUnionFields(x, typeof<'a>) with
        | case, _ -> case.Name
        | _ -> "UNKNOWN"

    let private tryParseUri (uri: string) : Uri option =
        match Uri.TryCreate(uri, UriKind.Absolute) with
        | (true, url) -> Some url
        | _ -> None

    type HttpMethod = GET | POST | PUT | DELETE | TRACE | PATCH | CONNECT | HEAD | OPTIONS
        with override this.ToString() = unionToString this

    type HttpHeader = string * string

    type HttpContext = {
        method: HttpMethod option;
        uri: Uri option;
        headers: HttpHeader list option;
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

    let private validateHttpContext(ctx: HttpContext) : HttpContext option =
        None
    let private createHttpRequest (ctx: HttpContext) : HttpResponse option =
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
            (* HTTP related Methods *)
        [<CustomOperation("keepalive", MaintainsVariableSpace=true)>]
        member this.Keepalive(ctx: HttpContext, keepalive: bool) =
            { ctx with keepalive = Some keepalive}
        [<CustomOperation("GET", MaintainsVariableSpace=true)>]
        member this.GET(ctx: HttpContext, uri: string, headers: (string * string) list) =
            { ctx with method = Some GET; uri = uri |> tryParseUri; headers = Some headers }

        member __.Run(ctx: HttpContext) =
            match ctx |> validateHttpContext with
            | Some ctx -> ctx |> createHttpRequest
            | None -> None

    let http = HttpBuilder()
    let response = http {
            timeout 10_000  // 10 seconds
            keepalive false
            GET "https://microsoft.com/" [
                ("X-Custom-Header", "HeaderValue")
            ]
        }
    // response |> printfn
