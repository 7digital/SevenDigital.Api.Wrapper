# Manually put a version of the Api wrapper on nuget

How to manually put a version of the 7digital c# Api wrapper on [nuget.org](http://www.nuget.org/).

##You will need:

- Your latest code merged to master.
- the nuget api key for pushing to [the wrapper package on nuget](https://www.nuget.org/packages/SevenDigital.Api.Wrapper/).
- Powershell, version 3 or later.

### Versioning

For more details on how and when to change version numbers see [Semantic Versioning](http://semver.org/)

When a version number is generated automatically, the last digit is incremented.  e.g. if the current version is `3.0.0` then the default next version number generated is `3.0.1`.  In general if the current released version is `x.y.z`, then the next version without any breaking changes is `x.y.(z+1)`. This is appropriate if there are no major changes.

The main reason for doing manual uploads to nuget is to override this number. You override this with the `-v` switch to the `buildpackage` script.

First, decide on a version number. 

## Preparation steps

* In git, make sure that you are on the master branch and have pulled the latest code, that you can build the code and it passes the unit tests. 

* Start Powershell and go to the `\buildpackage` folder.

* Run `.\buildpackage.ps1` This will do a dry run. Check that it worked.
* Try a dry run with the correct version number. e.g. if you want the version number to be `3.1.0` do  `.\buildPackage.ps1 -v "3.1.0"`
Check that it worked. 
* Try a run where you actually push, but push to a local folder, e.g. `.\buildPackage.ps1 -v "3.1.0" -s "c:\temp" -push true`
* Then check that you have the correct file in the output folder, in this case there should be a file at `C:\temp\SevenDigital.Api.Wrapper.3.1.0.nupkg`

## Pushing

If this is all OK and you are ready to push to nuget, do it for real, remove the `-s` params from the last command, e.g. enter
 `.\buildPackage.ps1 -v "3.1.0" -push true`


### Fixing errors.
Packages on nuget are regarded as immutable - they cannot be changed or hidden. But they can be hidden from new downloads. If you need to get rid of a package, log on to nuget and hide it. But try to avoid getting there.

## Prereleases

[Nuget supports prerelease versions](http://docs.nuget.org/docs/reference/versioning) by appending a dash and any string on the end of the version number.

Decide on a pre-release version number.  e.g. if the current version is `3.0.0` then the prerelease of the next version is `3.0.1-prerelease`. 
If this version exists already we use `prerelease2`, `prerelease3` etc. Nuget does not care what the appended string is, only that new versions sort later alphabetically.

In general if the current released version is `x.y.z`, then the next version without any breaking changes is `x.y.(z+1)` and the prerelease is `x.y.(z+1)-prerelease`.

Proceed as above using this version number. e.g. `.\buildPackage.ps1 -v "3.0.1-prerelease"`
