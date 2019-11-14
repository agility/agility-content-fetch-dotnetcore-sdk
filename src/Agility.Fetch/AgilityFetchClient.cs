using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Agility.Fetch {
	public class AgilityFetchClient {
		private readonly HttpClient _client;

		public AgilityFetchClient(HttpClient client) {
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ContentList<T>> ContentList<T>(AgilityContentListRequestBuilder builder) {
			using (var req = builder.Build())
			using (var resp = await _client.SendAsync(req, HttpCompletionOption.ResponseContentRead)) {
				var result = await ResponseHandler<ContentList<T>>(resp);
				return result;
			}
		}

		private async Task<T> ResponseHandler<T>(HttpResponseMessage resp) {
			if (resp.IsSuccessStatusCode) {
				using (var respStream = await resp.Content.ReadAsStreamAsync()) {
					var result = await JsonSerializer.DeserializeAsync<T>(respStream);
					return result;
				}
			} else {
				switch (resp.StatusCode) {
					case HttpStatusCode.Unauthorized:
						throw new UnauthorizedException();
					case HttpStatusCode.InternalServerError:
						throw new ServerErrorException();
					default:
						throw new AgilityFetchException(resp.StatusCode, "Unknown error");
				}
			}
		}
	}

	public class AgilityFetchException : Exception {
		public readonly HttpStatusCode StatusCode;

		public AgilityFetchException(HttpStatusCode statusCode, string message)
			: base(message) {
			StatusCode = statusCode;
		}
	}

	public class UnauthorizedException : AgilityFetchException {
		public UnauthorizedException() 
			: base(HttpStatusCode.Unauthorized, "Not authorized") { }
	}

	public class ServerErrorException : AgilityFetchException {
		public ServerErrorException()
			: base(HttpStatusCode.InternalServerError, "Agility experience an error") { }
	}
}
