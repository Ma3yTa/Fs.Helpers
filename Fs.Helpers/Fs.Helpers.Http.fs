// Until later...
namespace Fs.Helpers.Http

// module HttpHelpers =
//     open System
//     open System.Net.Http
//     open System.IO
//     open System.Threading.Tasks

//     open FSharp.Core

//     open Fs.Helpers.Option.OptionHelpers
//     open Fs.Helpers.Async.AsyncHelpers

//     let private defaultTimeout = double 60000 // 60 seconds

//     let private tryParseUri (uri: string) : Uri option =
//         match Uri.TryCreate(uri, UriKind.Absolute) with
//         | (true, url) -> Some url
//         | _ -> None

//     let private tryGetHeader (headers: Headers.HttpRequestHeaders seq) (header: string) : (string * string) option =
//         None

//     type HttpHeader = (string * string)

//     type HttpContext = {
//         method: HttpMethod option;
//         uri: Uri option;
//         headers: (HttpHeader list) option;
//         body: byte array option;
//         timeout: double option;
//         keepalive: bool option;
//     }

//     // TODO:
//     // Not sure that would be the proper way of "verifying" the context
//     // Maybe it's worth it to do some more "monadic" way
//     let private validateHttpContext(ctx: HttpContext) : HttpContext option =
//         match ctx with
//         | ctx when ctx.method = None || ctx.uri = None  -> None
//         | _ -> Some ctx

//     let private performHttpRequest (ctx: HttpContext) : Async<HttpResponseMessage> option =
//         use client = new HttpClient ()
//         use message = new HttpRequestMessage ()

//         // Assuming that both method & uri are set, as we did 'validateHttpContext' before
//         message.Method <- !!ctx.method
//         message.RequestUri <- !!ctx.uri

//         match ctx.headers with
//             | Some headers -> headers |> Seq.iter message.Headers.Add
//             | None -> ()

//         match ctx.body with
//             | Some body -> message.Content <- new ByteArrayContent(body)
//             | None -> ()

//         client.Timeout <- TimeSpan.FromMilliseconds (ctx.timeout |= defaultTimeout)

//         match ctx.keepalive with
//         | Some keepalive when keepalive -> client.DefaultRequestHeaders.Add ("Connection", "keep-alive")
//         | _ -> client.DefaultRequestHeaders.Add ("Connection", "close")

//         Some (message |> client.SendAsync |> Async.AwaitTask)

//     let inline taskResult (task: Task<'a>) = task.Result
//     let getResponse (response: Async<HttpResponseMessage> option) =
//         match response with
//         | Some res -> Some (sync res)
//         | None -> None

//     type HttpBuilder () =
//         member this.Zero() = {
//             method = None
//             uri = None
//             headers = None
//             body = None
//             timeout = None
//             keepalive = None
//         }
//         member this.Yield(_) = this.Zero()

//         (* HTTP related Methods *)
//         [<CustomOperation("timeout", MaintainsVariableSpace=true)>]
//         member inline this.Timeout(ctx: HttpContext, timeout) =
//             { ctx with timeout = Some (double timeout)}

//         [<CustomOperation("keepalive", MaintainsVariableSpace=true)>]
//         member this.Keepalive(ctx: HttpContext, keepalive: bool) =
//             { ctx with keepalive = Some keepalive}

//         [<CustomOperation("GET", MaintainsVariableSpace=true)>]
//         member this.GET(ctx: HttpContext, uri: string, headers: (string * string) list option) =
//             { ctx with method = Some HttpMethod.Get; uri = uri |> tryParseUri; headers = headers }

//         member this.Run(ctx: HttpContext) =
//             match ctx |> validateHttpContext with
//             | Some ctx -> ctx |> performHttpRequest
//             | None -> None

//     let http = HttpBuilder()
