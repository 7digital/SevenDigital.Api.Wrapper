echo "Building the nuget package for SevenDigital.Api.Wrapper"

del *.nupkg

nuget pack SevenDigital.Api.Wrapper.nuspec 

rem uncomment this to push to http://nuget.org
rem remember to set the 7digital api key first
rem NuGet Push SevenDigital.Api.Wrapper.nupkg

echo "done"

pause