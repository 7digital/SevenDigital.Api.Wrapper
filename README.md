![7digital](http://i.imgur.com/StUnvCy.png?1)

About 7digital
========

[7digital.com](7digital.com) is an online music store operating in over 16 countries and offering tens of millions of high quality DRM free MP3s (320kbps) from all major labels and wide range of independent labels and distributors. More details at [about.7digital.com](http://about.7digital.com/).

The 7digital API will give you access to the full catalogue including high quality album art, 30s preview clips for all tracks, commissions on sales, integrated purchasing and full length streaming. More details about the 7digital API at [developer.7digital.com](http://developer.7digital.com/)

What is this code?
========
The 7digital API Wrapper is a library to make it easy to access the 7digital API from C# code. 

You will need 
========

You will need a 7digital API key. If you don't have one, [sign up here](https://api-signup.7digital.com/).


The latest versions of the 7digital API Wrapper require .Net version 4.5.0 or later. If you are using .Net version 4.0, you can use a version of the wrapper numbered 3.x. 

When upgrading the wrapper from version 3.x to 4.x, see [the 4.0 release notes](https://github.com/7digital/SevenDigital.Api.Wrapper/blob/master/ReleaseNotes40.md) for the breaking changes and new additions. The main change is that the wrapper now returns awaitable tasks.

Getting the 7digital Api Wrapper
=====================

The latest released version of the 7digital API Wrapper is on nuget.org as [SevenDigital.Api.Wrapper](http://www.nuget.org/packages/SevenDigital.Api.Wrapper/). 

Install with the nuget package manager console: `Install-Package SevenDigital.Api.Wrapper`


We use [semantic versioning](http://semver.org/) for the version numbers of the package on nuget. We aim to release new versions promptly to nuget when needed, e.g. due to additions to the 7digital API or issues with the wrapper. 


Where to read more:
-----

See [the wiki](https://github.com/7digital/SevenDigital.Api.Wrapper/wiki) for further documentation on usage, contributing and so on.

Other resources:
----
* Schema used by this wrapper: [SevenDigital.Api.Schema](https://github.com/7digital/SevenDigital.Api.Schema).  
* Node.js: [Node.js API Client](https://github.com/raoulmillais/node-7digital-api).

Licence
----
This software is under the [MIT License (MIT)](http://opensource.org/licenses/MIT) and is Copyright (c) 2012 7digital Ltd.
