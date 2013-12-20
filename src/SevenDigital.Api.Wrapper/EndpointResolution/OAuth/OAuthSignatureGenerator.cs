using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public interface ISignatureGenerator
	{
		string Sign(OAuthSignatureInfo oAuthSignatureInfo);
		IDictionary<string, string> SignWithPostData(OAuthSignatureInfo oAuthSignatureInfo);
	}

	public class OAuthSignatureGenerator : ISignatureGenerator
	{
		private readonly OAuthBase _oAuthBase;

		public OAuthSignatureGenerator()
		{
			_oAuthBase = new OAuthBase();
		}

		public string Sign(OAuthSignatureInfo oAuthSignatureInfo)
		{
			var signedRequest = CreateSignedRequest(oAuthSignatureInfo);
			var encodedSignature = OAuthBase.UrlEncode(signedRequest.Signature);
			return string.Format("{0}?{1}&oauth_signature={2}", signedRequest.NormalizedUrl, signedRequest.NormalizedRequestParameters, encodedSignature);
		}

		public IDictionary<string, string> SignWithPostData(OAuthSignatureInfo oAuthSignatureInfo)
		{
			var signedRequest = CreateSignedRequest(oAuthSignatureInfo);

			var parameters = new Dictionary<string, string>(oAuthSignatureInfo.PostData)
			{
				{ OAuthBase.OAuthVersionKey, OAuthBase.OAuthVersion },
				{ OAuthBase.OAuthNonceKey, signedRequest.Nonce },
				{ OAuthBase.OAuthTimestampKey, signedRequest.Timestamp },
				{ OAuthBase.OAuthSignatureMethodKey, OAuthBase.HMACSHA1SignatureType },
				{ OAuthBase.OAuthConsumerKeyKey, oAuthSignatureInfo.ConsumerCredentials.ConsumerKey },
				{ OAuthBase.OAuthSignatureKey, OAuthBase.UrlEncode(signedRequest.Signature) }
			};

			if (oAuthSignatureInfo.UserAccessToken != null && !string.IsNullOrEmpty(oAuthSignatureInfo.UserAccessToken.Token))
			{
				parameters.Add(OAuthBase.OAuthTokenKey, OAuthBase.UrlEncode(oAuthSignatureInfo.UserAccessToken.Token));
			}

			return parameters;
		}

		private SignedRequest CreateSignedRequest(OAuthSignatureInfo oAuthSignatureInfo)
		{
			var timestamp = _oAuthBase.GenerateTimeStamp();
			var nonce = _oAuthBase.GenerateNonce();

			string normalizedRequestParameters;
			string normalizedUrl;
			
			var signature = _oAuthBase.GenerateSignature(new Uri(oAuthSignatureInfo.FullUrlToSign),
			                                             oAuthSignatureInfo.ConsumerCredentials.ConsumerKey,
			                                             oAuthSignatureInfo.ConsumerCredentials.ConsumerSecret,
			                                             oAuthSignatureInfo.UserAccessToken.Token,
			                                             oAuthSignatureInfo.UserAccessToken.Secret,
			                                             oAuthSignatureInfo.HttpMethod,
			                                             timestamp,
			                                             nonce,
			                                             out normalizedUrl,
			                                             out normalizedRequestParameters,
			                                             oAuthSignatureInfo.PostData);

			return new SignedRequest
			{
				NormalizedRequestParameters = normalizedRequestParameters,
				NormalizedUrl = normalizedUrl,
				Signature = signature,
				Timestamp = timestamp,
				Nonce = nonce
			};
		}
	}
}