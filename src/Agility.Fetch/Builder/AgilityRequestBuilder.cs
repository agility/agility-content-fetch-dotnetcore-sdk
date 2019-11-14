using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Agility.Fetch {
	public abstract class AgilityRequestBuilder {
		const string _apiKeyHeader = "APIKey";
		const string _guidHeader = "guid";

		readonly string _apiKey;
		readonly string _guid;
		readonly FetchMode _fetchMode;
		readonly string _base_url;

		protected AgilityRequestBuilder(
			string apiKey,
			string guid,
			FetchMode fetchMode = FetchMode.Live,
			string baseUrl = null) {
			_apiKey = apiKey;
			_guid = guid;
			_fetchMode = fetchMode;
			_base_url = TrySanitizeBaseUrl(baseUrl, out string url) ? url : DefaultBaseUrl;
		}

		string DefaultBaseUrl =>
			$"https://{_guid}-api.agilitycms.cloud";

		protected HttpRequestMessage BuildRequestMessage(Uri uri) {
			var req = new HttpRequestMessage(HttpMethod.Get, uri);

			req.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
			req.Headers.Add(_apiKeyHeader, _apiKey);

			if (!string.IsNullOrWhiteSpace(_guid)) {
				req.Headers.Add(_guidHeader, _guid);
			}

			return req;
		}

		protected Uri CreateRequestBaseUri() {
			var b = new UriBuilder(new Uri(_base_url));
			string mode = _fetchMode == FetchMode.Live ? "fetch" : "preview";			
			b.Path = Path.Combine(b.Path, mode);
			return b.Uri;
		}

		private bool TrySanitizeBaseUrl(string baseUrl, out string sanitized) {
			if (string.IsNullOrWhiteSpace(baseUrl)) {
				sanitized = string.Empty;
				return false;
			}

			sanitized = baseUrl.TrimEnd('/');
			return true;
		}

	}
}

