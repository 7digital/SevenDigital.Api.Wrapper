# Manually put a version of the Api wrapper on nuget

How to manually put a version of the 7digital c# Api wrapper on nuget.

##You will need:

- Your latest code merged to master.
- the nuget api key for pushing to [the wrapper library on nuget](https://www.nuget.org/packages/SevenDigital.Api.Wrapper/).
- powershell


Decide on a version number. e.g. if the current version is `3.0.0` then the default next version number generated is `3.0.1`.  

In general if the current released version is `x.y.z`, then the next version without any breaking changes is `x.y.(z+1)` If there are breaking changes then you can override this with the `-v` switch.

For more details on how and when to change version numbers see [SemVer](http://semver.org/)

## Preparation steps

* In git, make sure that you are on the master branch and have pulled the latest code. 

* Start powershell and go to the `\buildpackage` folder.

* Run `.\buildpackage.ps1` This will do a dry run. Check that it worked.
* Try a dry run with the correct version number. e.g. if you want the version number to be `3.1.0` do  `.\buildPackage.ps1 -v "3.1.0"`
Check that it worked. 
* Try a run where you actually push, but to a local folder, e.g. `.\buildPackage.ps1 -v "3.1.0" -s "c:\temp" -push true`
* Then check that you have the correct file in the output folder, in this case the file `C:\temp\SevenDigital.Api.Wrapper.3.1.0.nupkg`

## Pushing

If this is all OK and you are ready to push to nuget, do it for real, remove the `-s` params from the last command, e.g. enter
 `.\buildPackage.ps1 -v "3.1.0" -push true`


If you get a version number wrong or otherwise don't want that package used, you can hide it on nuget but not delete it.



## Prereleases

Decide on a pre-release version number. 

e.g. if the current version is `3.0.0` then the prerelease version number is `3.0.1-prerelease`. 
If this version exists already we use `prerelease2` `prerelease3` etc. Nuget only cares that new versions sort later alphabetically.

In general if the current released version is `x.y.z`, then the next version without any breaking changes is `x.y.(z+1)` and the prerelease is `x.y.(z+1)-prerelease`.


Proceed as above using this version number. e.g. `.\buildPackage.ps1 -v "3.0.1-prerelease"`
