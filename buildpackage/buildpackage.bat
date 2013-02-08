echo "Building the nuget package for SevenDigital.Api.Wrapper."

del *.nupkg

nuget pack SevenDigital.Api.Wrapper.nuspec 

rem NuGet Push SevenDigital.Api.Wrapper.nupkg

echo "done"

pause