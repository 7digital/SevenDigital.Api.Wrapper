![7digital](http://i.imgur.com/StUnvCy.png?1)

About 7digital
========

[7digital.com](7digital.com) is an online music store operating in over 16 countries and offering tens of millions of high quality DRM free MP3s (320kbps) from all major labels and wide range of independent labels and distributors. More details at [about.7digital.com](http://about.7digital.com/).

The 7digital API will give you access to the full catalogue including high quality album art, 30s preview clips for all tracks, commissions on sales, integrated purchasing and full length streaming. More details about the 7digital API at [developer.7digital.com](http://developer.7digital.com/)

What is this code
========
the 7digital API Wrapper is a library to make it easy to access the 7digital API from C# code. 

You will need 
========

You will need a 7digital API key. If you don't have one, [sign up here](https://api-signup.7digital.com/).


The 7digital API Wrapper requires .Net version 4.5.0 or later. If you are using .Net version 4.0, you can use a version of the wrapper numbered 3.x. 

When upgrading the wrapper from version 3.x to 4.x, see [the 4.0 release notes](https://github.com/7digital/SevenDigital.Api.Wrapper/blob/master/ReleaseNotes40.md) for the breaking changes and new additions. The main change is that the wrapper now returns tasks to used with `await`.

Getting the 7digital Api Wrapper
=====================

The latest released version of the 7digital API Wrapper is on nuget.org as [SevenDigital.Api.Wrapper](http://www.nuget.org/packages/SevenDigital.Api.Wrapper/). 

Install with the nuget package manager console: `Install-Package SevenDigital.Api.Wrapper`


We use [semantic versioning](http://semver.org/) for the version numbers of the package on nuget. We aim to release new versions promptly when needed due to wrapper issues or changes to the 7digital API.  Whilst we make every effort to keep Master as stable as possible, we cannot 
guarantee that it is always in a usable state as active
development may be happening. 



Usage
-----

Consuming applications need to provide a concrete implementation of `IApiUri` and `IOAuthCredentials` 
in order to authenticate with the 7digital Api. Otherwise wrapper will throw `MissingDependencyException`.

Current invocation:

artist/details endpoint

    Artist artist = await Api<Artist>
        .Create
        .WithArtistId(1)
        .Please();

Release charts endpoint:   
 
    var request = Api<ReleaseChart>.Create;
    var chart = await request.Please();

Handling Errors
---------------

There are a number of reasons you may experience an error when using the API wrapper. 
All exceptions inherit from `ApiException` but it is probably not a good idea to have 
a blanket catch all for these exceptions when calling the wrapper as it will mask potential
other issues.  

These conditions can be broken down into 2 categories, those which indicate an error thrown
in the protocols which the API relies on (TCP, HTTP, OAuth) and those which are indicated by
an error status response from the API:

### API errors

These are represented as classes derived from `ApiErrorException`

* The release you are requesting no longer exists or is not available in the territory
* You supplied an incorrect value for a parameter
etc etc

You should catch and inspect each of the relevant types of error at the callsite for the
endpoint you are calling and take appropriate action. See the [API documentation](http://api.7digital.com/1.2/static/documentation/7digitalpublicapi.html#Error_responses)
 for a full list.  Each range of error codes maps to a different exception type.

### HTTP errors

These will be either `NonXmlResponseException`s or `OAuthException`s

* The API is down for maintenance or failing in some way
* The OAuth credentials you have supplied are not valid
* The OAuth token you are using has expired
* You have overriden a URL with something which does not exist
etc etc

A generic application catchall for `NonXmlResponseException` is sensible as it indicates
problems with the 7digital API.  

`OAuthException`s will require special attention and testing as it could be either a problem
with the user's access token, the user not authenticating your application or an attempt to
access premium API resources without your application being granted the correct permissions.

Notes
-----

A Schema type knows about its endpoint via the [ApiEndpoint] attribute, e.g

    [ApiEndpoint("artist/details")]
    public class Artist{}

Also - you can apply the following to a type to specify if the endpoint requires 
oauth:

    [OAuthSigned]
    public class OAuthRequestToken{}

See example usage console app project for some more examples.
