using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Agility.Fetch.Models;
using Newtonsoft.Json;

namespace Agility.Fetch
{
	public class AgilityFetchClient
	{
		private static HttpClient _staticHttpClient;
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;
		private readonly FetchMode _fetchMode;
		private readonly string _baseUrl;
		private readonly string _guid;

		public AgilityFetchClient(string baseUrl, string guid, string apiKey, FetchMode fetchMode)
		{
			if (_staticHttpClient == null)
			{
				_staticHttpClient = new HttpClient();
			}

			_httpClient = _staticHttpClient;
			_apiKey = apiKey;
			_fetchMode = fetchMode;
			_guid = guid;

			string typeStr = fetchMode == FetchMode.Live ? "fetch" : "preview";
			if (baseUrl.EndsWith("/")) baseUrl = baseUrl.TrimEnd('/');
			_baseUrl = $"{baseUrl}/{typeStr}";

		}
		
		private Dictionary<string, string> GlobalCDNConfigurations = new Dictionary<string, string>
		{
			{"-c", "-ca"},
			{"-d", "-dev"},
			{"-u", ""}
		};

		public AgilityFetchClient(string guid, string apiKey, FetchMode fetchMode)
		{
			if (_staticHttpClient == null)
			{
				_staticHttpClient = new HttpClient();
			}

			if (string.IsNullOrEmpty(guid))
			{
				throw new ArgumentException("Invalid guid provided");
			}

			if (string.IsNullOrEmpty(apiKey))
			{
				throw new ArgumentException("Invalid apiKey provided");
			}

			var suffix = guid.Substring(guid.Length - 2);

			_httpClient = _staticHttpClient;
			_apiKey = apiKey;
			_fetchMode = fetchMode;
			
			string typeStr = fetchMode == FetchMode.Live ? "fetch" : "preview";

			if (suffix.StartsWith("-") && GlobalCDNConfigurations.TryGetValue(suffix.ToLowerInvariant(), out var config))
			{
				_baseUrl = $"https://api{config}.aglty.io/{typeStr}";
			}
			else
			{
				_baseUrl = $"https://{guid}-api.agilitycms.cloud/{typeStr}";
			}
			
			

			

		}
		private void AuthorizeRequest(HttpRequestMessage request)
		{
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
			request.Headers.Add("APIKey", _apiKey);

			if ( ! string.IsNullOrWhiteSpace(_guid))
			{
				request.Headers.Add("guid", _guid);
			}
		}

		private async Task<T> ProcessResponse<T>(HttpResponseMessage response)
		{
			var status = response.StatusCode;
			string responseData = null;

			try
			{
				responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			}
			catch { }

			if (response.IsSuccessStatusCode)
			{
				//if we just want the string back...
				if (typeof(T) == typeof(string))
				{
					return (T)(object)responseData;
				}

				try
				{
					var result = JsonConvert.DeserializeObject<T>(responseData);
					return result;
				}
				catch (Exception exception)
				{
					throw new ApplicationException("Could not deserialize the response body.", exception);
				}
			}
			else
			{

			}

			switch (status)
			{
				case HttpStatusCode.Unauthorized:
					throw new ApplicationException("Agility returned a 401 not authorized.");
				case HttpStatusCode.InternalServerError:
					throw new ApplicationException("Agility returned a 500 server error.");
				default:
					throw new ApplicationException($"Agility returned a {(int)status} server response: {responseData}");
			}
		}

		public async Task<ListResponse<T>> List<T>(string languageCode, string referenceName, int skip= 0, int take = 50, IList<string> fields = null, int contentLinkDepth = 0, string filter = null)
		{

			if (! string.IsNullOrWhiteSpace(filter))
			{
				filter = System.Net.WebUtility.UrlEncode(filter);
			}

			string url = $"{_baseUrl}/{languageCode.ToLowerInvariant()}/list/{referenceName.ToLowerInvariant()}?skip={skip}&take={take}&contentLinkDepth={contentLinkDepth}&filter={filter}";
			if (fields != null && fields.Count > 0)
			{
				url += "&fields=" + string.Join<string>(".", fields);
			}

			using (var request = new HttpRequestMessage(HttpMethod.Get, url))
			{
				
				AuthorizeRequest(request);
				
				using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead))
				{
					var result = await ProcessResponse<ListResponse<T>>(response);
					return result;
				}

			}
		}
	}
}
