using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Agility.Fetch {
	public sealed class AgilityContentListRequestBuilder : AgilityRequestBuilder {
		public AgilityContentListRequestBuilder(
			string apiKey,
			string guid,
			FetchMode fetchMode = FetchMode.Live,
			string baseUrl = null) : base(apiKey, guid, fetchMode, baseUrl) {
			Skip = 0;
			Take = 10;
			ContentLinkDepth = 1;			
			EncodedFilter = string.Empty;
		}

		string LanguageCode { get; set; }
		string ReferenceName { get; set; }
		int Skip { get; set; }
		int Take { get; set; }
		int ContentLinkDepth { get; set; }		
		string EncodedFilter { get; set; }
		
		public HttpRequestMessage Build() {
			if (string.IsNullOrWhiteSpace(LanguageCode))
				throw new ArgumentNullException(nameof(LanguageCode));

			if (string.IsNullOrWhiteSpace(ReferenceName))
				throw new ArgumentNullException(nameof(ReferenceName));

			return base.BuildRequestMessage(CreateRequestUri());
		}

		public AgilityContentListRequestBuilder SetContentLinkDepth(int contentLinkDepth){
			ContentLinkDepth = contentLinkDepth < 1 ? 1 : contentLinkDepth;
			return this;
		}

		public AgilityContentListRequestBuilder SetFilter(AgilityContentFilterBuilder builder) {
			EncodedFilter = builder.Build();
			return this;
		}

		public AgilityContentListRequestBuilder SetLanguage(string languageCode) {
			LanguageCode = languageCode;
			return this;
		}

		public AgilityContentListRequestBuilder SetReferenceName(string referenceName) {
			ReferenceName = referenceName;
			return this;
		}

		public AgilityContentListRequestBuilder SetSkip(int skip) {
			Skip = skip < 0 ? 0 : skip;
			return this;
		}

		public AgilityContentListRequestBuilder SetTake(int take) {
			Take = take > 50 ? 50 : take;
			return this;
		}

		Uri CreateRequestUri() {
			var sb = new StringBuilder();

			//language
			sb.Append("/");
			sb.Append(LanguageCode);

			//request type
			sb.Append("/list");

			//reference type
			sb.Append("/");
			sb.Append(ReferenceName.ToLowerInvariant());

			//skip
			sb.Append("?skip=");
			sb.Append(string.Format("{0}", Skip));

			//take
			sb.Append("&take=");
			sb.Append(string.Format("{0}", Take));

			//content link depth
			sb.Append("&contentLinkDepth=");
			sb.Append(string.Format("{0}", ContentLinkDepth));

			//filter
			if (!string.IsNullOrWhiteSpace(EncodedFilter)) {
				sb.Append("&filter=");
				sb.Append(EncodedFilter);
			}
			
			return new Uri(CreateRequestBaseUri(), sb.ToString()); ;
		}
	}

}
