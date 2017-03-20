// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake

// TODO: Add AssemblyInfo generating
// TODO: Add Test Target
// TODO: Add BuildNuget target && PublishNuget target
// TODO: Add docs generating target
// TODO: Define both Release & Debug configs here

// Directories
let buildDir  = "./build/"
let deployDir = "./deploy/"


// Filesets
let appReferences  =
    !! "/**/*.csproj"
    ++ "/**/*.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; deployDir]
)

Target "Build" (fun _ ->
    // compile all projects below src/app/
    MSBuildDebug buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

Target "Test" DoNothing

Target "All" DoNothing

"All"
  ==> "Clean"
  ==> "Build"
  ==> "Test"

RunTargetOrDefault "All"
