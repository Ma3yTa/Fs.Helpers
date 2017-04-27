// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

// TODO: Add AssemblyInfo generating
// TODO: Add Test Target
// TODO: Add BuildNuget target && PublishNuget target
// TODO: Add docs generating target
// TODO: Define both Release & Debug configs here

// Directories
let buildDir  = "./build/"
let deployDir = "./deploy/"
let testDir = "./test/"
let nunitConsolePath = System.IO.Path.GetFullPath "packages" </> "test" </> "NUnit.ConsoleRunner" </> "tools" </> "nunit3-console.exe"


// Filesets
let appReferences  = !! "/**/*.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; deployDir; testDir]
)

Target "Build" (fun _ ->
    MSBuildDebug buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    !! (buildDir + "*Test?.dll")
    |> NUnit3 (fun p -> { p with OutputDir = testDir + "Results.xml"; ToolPath = nunitConsolePath })
)

Target "All" DoNothing

"Clean" ==> "Build" ==> "Test" ==> "All"

RunTargetOrDefault "All"