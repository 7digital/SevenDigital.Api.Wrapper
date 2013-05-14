function ReadLinesFromFile([string] $fileName)
{
 [string]::join([environment]::newline, (get-content -path $fileName))
}

function BuildSolution
{
  [CmdletBinding()]
  param()
  C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe ..\SevenDigital.Api.Wrapper.sln /t:build /p:Configuration=Debug
}

function GetLatestFullVersionOnNuget()
{
  [CmdletBinding()]
  param()

   $packageDetails = &nuget list SevenDigital.Api.Wrapper
   $parts = $packageDetails.Split(' ')
   [string]$parts[1]
}

function GetLastVersionNumber()
{
  [CmdletBinding()]
  param()

  $fullVersion = GetLatestFullVersionOnNuget
  $parts = $fullVersion.Split('.')
  [int]$parts[2]
}

function CleanupBuildArtifacts
{
  [CmdletBinding()]
  param()
  del SevenDigital.Api.Wrapper.nuspec
  del *.nupkg
}


BuildSolution

$nextVersionNumber =  (GetLastVersionNumber) + 1
$fullVersion = "2.0.$nextVersionNumber"
write-output "Next package version: $fullVersion"

# make the nuspec file with the target version number
$nuspecTemplate = ReadLinesFromFile "SevenDigital.Api.Wrapper.nuspec.template"
$nuspecWithVersion = $nuspecTemplate.Replace("#version#", $fullVersion)
$nuspecWithVersion > SevenDigital.Api.Wrapper.nuspec

nuget pack SevenDigital.Api.Wrapper.nuspec 

$pushCommand = "NuGet Push SevenDigital.Api.Wrapper.#version#.nupkg".Replace("#version#", $fullVersion)

# push to nuget:
#write-output "Push command is $pushCommand"
Invoke-Expression $pushCommand
write-output "Pushed package version $nextVersion"

CleanupBuildArtifacts
write-output "Done"
