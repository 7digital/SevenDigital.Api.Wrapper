23/05/2012

A note on branches:
 
Master is not always going to be stable. If you want to use a stable version please use the latest versioned tag (v1.x.x)

03/06/2011

-- IMPORTANT --
Consuming applications require to provide a concrete implementation of IApiUri and IOAuthCredentials 
in order to authenticate with the 7digital API. Otherwise wrapper will throw MissingDependencyException.

Current usage:

artist/details endpoint
Artist artist = Api<Artist>.Get
		.WithArtistId(1)
		.Please()

Not all endpoints implemented yet, just Artist and some of Release.

Type knows about its endpoint via the [ApiEndpoint] attribute, e.g

[ApiEndpoint("artist/details")]
public class Artist{}

Also - you can apply the following to a type to specify if the endpoint requires oauth
[OAuthSigned]
public class OAuthRequestToken{}

See example usage console app project for some more examples.