function ReadLinesFromFile([string] $fileName)
{
 [string]::join([environment]::newline, (get-content -path $fileName))
}

function GetNextVersionNumber
{
  $lastVersionText = get-content lastVersion.txt
  $lastVersion = [int]$lastVersionText
  $lastVersion + 1
}

function CleanupBuildArtifacts
{
  del SevenDigital.Api.Wrapper.nuspec
  del *.nupkg
}

function UpdateVersionNumber([int] $newVersionNumber)
{
   $newVersionNumber > lastVersion.txt

  # write changed version number back to git
  # git add lastVersion.txt
  # git commit -m "automated package build and version number increment"
  # git push
}

$nextVersionNumber = GetNextVersionNumber
$fullVersion = "2.0.$nextVersionNumber"
write-output "Next package version: $fullVersion";

# make the nuspec file with the target version number
$nuspecTemplate = ReadLinesFromFile "SevenDigital.Api.Wrapper.nuspec.template"
$nuspecWithVersion = $nuspecTemplate.Replace("#version#", $fullVersion)
$nuspecWithVersion > SevenDigital.Api.Wrapper.nuspec

nuget pack SevenDigital.Api.Wrapper.nuspec 

$pushCommand = "NuGet Push SevenDigital.Api.Wrapper.#version#.nupkg".Replace("#version#", $fullVersion)

# push to nuget:
# & $pushCommand
#write-output "Pushed package version $nextVersion";
write-output "could have run: $pushCommand"

CleanupBuildArtifacts
UpdateVersionNumber $nextVersionNumber

write-output "Done";
