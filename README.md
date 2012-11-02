Branches and Releases
=====================

Whilst we make every effort to keep Master as stable as possible. We cannot 
guarantee, it to be in a permanently usable state as this is where active
development is happening. 

We aim to release semver versioned tags regularly. If you want to use a stable 
version please use the latest versioned tag (v1.x.x)

Work is currently happening towards a v2.x.x release on the master branch with
breaking API changes.

Usage
-----

Consuming applications need to provide a concrete implementation of IApiUri and 
IOAuthCredentials in order to authenticate with the 7digital API. Otherwise wrapper 
will throw MissingDependencyException.

Current invocataion:

artist/details endpoint

    Artist artist = Api<Artist>
                        .Create
                        .WithArtistId(1)
                        .Please()

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
