namespace Fs.Helpers.Http
    module HttpExtensions =
        open System
        open FSharp.Core
        open Fs.Helpers.Types.DateTimeTypes
        type HttpMethod = GET | POST | HEAD | OPTIONS
        type HttpHeader = string * string
        type HttpContext = {
            method: HttpMethod option;
            uri: Uri option;
            headers: HttpHeader list option;
            body: byte array option;
            timeout: int option
        }
        let private tryParseUri (uri: string) : Uri option =
            match Uri.TryCreate(uri, UriKind.Absolute) with
            | (true, url) -> Some url
            | _ -> None

        type HttpBuilder () =
            member this.Zero() = {
                method = None
                uri = None
                headers = None
                body = None
                timeout = None
            }
            member this.Yield(_) = this.Zero()
            (* HTTP related Methods *)
            [<CustomOperation("timeout", MaintainsVariableSpace=true)>]
            member this.Timeout(ctx: HttpContext, timeout: int) =
                { ctx with timeout = Some timeout}
            [<CustomOperation("GET", MaintainsVariableSpace=true)>]
            member this.GET(ctx: HttpContext, uri: string, headers: (string * string) list) =
                { ctx with method = Some GET; uri = uri |> tryParseUri; headers = Some headers }

            member __.Run(ctx: HttpContext) = 42

        let http = HttpBuilder()
        let response = http {
                timeout 10_000 // 10 seconds
                GET "https://microsoft.com/" [
                    ("X-Custom-Header", "HeaderValue")
                ]
            }
        // response |> printfn

        let test2 (x: (string * string) list) : int = 0

