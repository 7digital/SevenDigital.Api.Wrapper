using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SevenDigital.Api.Wrapper.Environment;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Requests.Serializing;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper
{
	public class FluentApi<T> : IFluentApi<T> where T : class, new()
	{
		private IHttpClient _httpClient;
		private readonly IRequestBuilder _requestBuilder;

		private readonly RequestData _requestData;
		private readonly IResponseParser _parser;
		private IResponseCache _responseCache;

		private readonly List<IPayloadSerializer> _payloadSerializers = new List<IPayloadSerializer>
			{
				new XmlPayloadSerializer(),
				new JsonPayloadSerializer(),
				new FormUrlEncodedPayloadSerializer()
			};

		public FluentApi(IHttpClient httpClient, IRequestBuilder requestBuilder, 
			IResponseParser responseParser, IResponseCache responseCache)
		{
			_httpClient = httpClient;
			_requestBuilder = requestBuilder;
			_parser = responseParser;
			_responseCache = responseCache;

			var attributeValidation = new AttributeRequestDataBuilder<T>();
			_requestData = attributeValidation.BuildRequestData();
		}

		public FluentApi(IHttpClient httpClient, IRequestBuilder requestBuilder, IResponseParser responseParser)
			: this(httpClient, requestBuilder, responseParser, new NullResponseCache())
		{}

		public IFluentApi<T> UsingClient(IHttpClient httpClient)
		{
			if (httpClient == null)
			{
				throw new ArgumentNullException("httpClient");
			}

			_httpClient = httpClient;
			return this;
		}

		public IFluentApi<T> UsingCache(IResponseCache responseCache)
		{
			if (responseCache == null)
			{
				throw new ArgumentNullException("responseCache");
			}

			_responseCache = responseCache;
			return this;
		}

		public IFluentApi<T> UsingBaseUri(IBaseUriProvider baseUriProvider)
		{
			if (baseUriProvider == null)
			{
				throw new ArgumentNullException("baseUriProvider");
			}

			_requestData.BaseUriProvider = baseUriProvider;
			return this;
		}

		public IFluentApi<T> WithMethod(HttpMethod httpMethod)
		{
			_requestData.HttpMethod = httpMethod;
			return this;
		}

		public IFluentApi<T> WithAccept(AcceptFormat acceptFormat)
		{
			if (acceptFormat == null)
			{
				throw new ArgumentNullException("acceptFormat");
			}

			_requestData.Accept = acceptFormat.ToString();
			return this;
		}

		public IFluentApi<T> WithParameter(string parameterName, string parameterValue)
		{
			_requestData.Parameters[parameterName] = parameterValue;
			return this;
		}

		public IFluentApi<T> ForUser(string oAuthToken, string oAuthTokenSecret)
		{
			_requestData.OAuthToken = oAuthToken;
			_requestData.OAuthTokenSecret = oAuthTokenSecret;
			return this;
		}

		public IFluentApi<T> WithPayload(string contentType, string payload)
		{
			_requestData.Payload = new RequestPayload(contentType, payload);
			return this;
		}

		public IFluentApi<T> WithPayload<TPayload>(TPayload payload) where TPayload : class
		{
			return WithPayload(payload, PayloadFormat.Xml);
		}

		public IFluentApi<T> WithPayload<TPayload>(TPayload payload, PayloadFormat payloadFormat) where TPayload : class
		{
			var correctSerializer = _payloadSerializers.FirstOrDefault(payloadSerializer => payloadSerializer.Handles == payloadFormat);

			_requestData.Payload = new RequestPayload(correctSerializer.ContentType, correctSerializer.Serialize(payload));
			return this;
		}

		public async Task<Response> Response()
		{
			var request = _requestBuilder.BuildRequest(_requestData);
			return await Response(request);
		}

		private async Task<Response> Response(Request request)
		{
			try
			{
				return await _httpClient.Send(request);
			}
			catch (WebException webException)
			{
				throw new ApiWebException(webException.Message, webException, request);
			}
		}

		public async Task<TR> ResponseAs<TR>() where TR: class, new()
		{
			var request = _requestBuilder.BuildRequest(_requestData);

			try
			{
				var response = await _httpClient.Send(request);
				var responseDeserializer = new ResponseDeserializer();
				return responseDeserializer.DeserializeResponse<TR>(response, false);
			}
			catch (WebException webException)
			{
				throw new ApiWebException(webException.Message, webException, request);
			}
		}
		public async Task<T> Please()
		{
			T cachedResult;
			var request = _requestBuilder.BuildRequest(_requestData);
			var foundInCache = _responseCache.TryGet(request, out cachedResult);
			if (foundInCache)
			{
				return cachedResult;
			}

			Response response = await Response(request);
			var result = _parser.Parse<T>(response);
			// set to cache only after all validation and parsing has succeeded
			_responseCache.Set(response, result);

			return result;
		}

		public string EndpointUrl
		{
			get
			{
				var request = _requestBuilder.BuildRequest(_requestData);
				return request.Url;
			}
		}

		public IDictionary<string, string> Parameters
		{
			get { return _requestData.Parameters; }
		}
	}
}