# How to put a version of the Api wrapper on nuget

How to put a version of the 7digital c# Api wrapper on [nuget.org](http://www.nuget.org/).

##You will need:

- Your latest code merged to [master on github](https://github.com/7digital/SevenDigital.Api.Wrapper).
- the nuget api key for pushing to [the wrapper package on nuget](https://www.nuget.org/packages/SevenDigital.Api.Wrapper/). You can retrieve this when logged into nuget with an authorised account.
- PowerShell, version 3 or later.

For more details consult [the wiki](https://github.com/7digital/SevenDigital.Api.Wrapper/wiki) or colleagues.

### Versioning

First, decide on a version number.  We use [Semantic Versioning](http://semver.org/).

In general, if the current released version is `x.y.z`, then the next version with bug fixes is `x.y.(z+1)` and with non-breaking additions is  `x.(y+1).0`.

E.g. if the current version is `2.1.3`. A version with a bugfix would be `2.1.4` and with a new non-breaking feature would be `2.2.0`.

Note that the version are numbers not strings, i.e. the next minor version after `2.1.9` is `2.1.10`.

For more details see [Semantic Versioning](http://semver.org/).

## Preparation steps

* In your local copy of the code, use git. Make sure that you are on the master branch and have pulled the latest code from [the official master](https://github.com/7digital/SevenDigital.Api.Wrapper), that you can build the code and it passes the unit tests. 

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
Packages on nuget are regarded as immutable - they cannot be changed or deleted. But they can be hidden from new downloads. If you need to get rid of a package, log on to nuget and hide it. But try to avoid getting to this state.

## Prereleases

[Nuget supports prerelease versions](http://docs.nuget.org/docs/reference/versioning) by appending a dash and any string on the end of the version number. In practice, pre-releases are more often used for testing major or breaking changes that accompany a major version number change.

Decide on a prerelease version number.  e.g. if the current version is `3.1.10` then the pre-release of the next major version is `4.0.0-prerelease`. 
If we issue multiple prereleases then we use `prerelease2`, `prerelease3` etc. Nuget does not care what the appended string is, only that new versions sort later alphabetically.

In general if the current released version is `x.y.z`, then the next major version without any breaking changes is `(x+1).0.0` and the prerelease is `(x+1).0.0-prerelease`.

Proceed as above using this version number. e.g. `.\buildPackage.ps1 -v "4.0.0-prerelease"`
