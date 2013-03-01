echo "Building the nuget package for SevenDigital.Api.Wrapper"

del *.nupkg

nuget pack SevenDigital.Api.Wrapper.nuspec 

rem remember to set the 7digital api key first
rem uncomment the next line to push to http://nuget.org
rem remember to set the version number here and the the .nupkg - or automate incrementing this?
rem NuGet Push SevenDigital.Api.Wrapper.2.0.3.nupkg

echo "done"

pause