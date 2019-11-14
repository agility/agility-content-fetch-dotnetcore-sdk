using System;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Agility.Fetch.Tests {
	public class AgilityContentListRequestBuilderTests {
		const string _apiKey = "defaultlive.201ffdd0841cacad5bb647e76547e918b0c9ecdb8b5ddb3cf92e9a79b03623cb";
		const string _guid = "ade6cf3c";

		[Fact]
		public void EmptyBuilderShouldThrow() {
			var builder = GetDefaultBuilder();
			Assert.Throws<ArgumentNullException>(builder.Build);
		}

		[Fact]
		public void NoLanguageShouldThrow() {
			var builder = GetDefaultBuilder().SetReferenceName("posts");
			Assert.Throws<ArgumentNullException>(builder.Build);
		}

		[Fact]
		public void NoReferenceNameShouldThrow() {
			var builder = GetDefaultBuilder().SetLanguage("en-us");
			Assert.Throws<ArgumentNullException>(builder.Build);
		}

		[Fact]
		public void DefaultSettingsBuilderTest() {
			var builder = GetDefaultConfiguredBuilder();

			var req = builder.Build();

			Assert.Equal(HttpMethod.Get, req.Method);
			Assert.Equal(_apiKey, req.Headers.GetValues("APIKey").FirstOrDefault());
			Assert.Equal(_guid, req.Headers.GetValues("guid").FirstOrDefault());
			Assert.Equal("https://ade6cf3c-api.agilitycms.cloud/en-us/list/posts?skip=0&take=10&contentLinkDepth=1", req.RequestUri.AbsoluteUri);
		}

		[Fact]
		public void SkipOneBuilderTest() {
			var builder = GetDefaultConfiguredBuilder().SetSkip(1);

			var req = builder.Build();
			Assert.Equal(HttpMethod.Get, req.Method);
			Assert.Equal(_apiKey, req.Headers.GetValues("APIKey").FirstOrDefault());
			Assert.Equal(_guid, req.Headers.GetValues("guid").FirstOrDefault());
			Assert.Equal("https://ade6cf3c-api.agilitycms.cloud/en-us/list/posts?skip=1&take=10&contentLinkDepth=1", req.RequestUri.AbsoluteUri);
		}

		[Fact]
		public void SkipMinusOneBuilderTest() {
			var builder = GetDefaultConfiguredBuilder().SetSkip(-1);

			var req = builder.Build();
			Assert.Equal(HttpMethod.Get, req.Method);
			Assert.Equal(_apiKey, req.Headers.GetValues("APIKey").FirstOrDefault());
			Assert.Equal(_guid, req.Headers.GetValues("guid").FirstOrDefault());
			Assert.Equal("https://ade6cf3c-api.agilitycms.cloud/en-us/list/posts?skip=0&take=10&contentLinkDepth=1", req.RequestUri.AbsoluteUri);
		}

		[Fact]
		public void Take50BuilderTest() {
			var builder = GetDefaultConfiguredBuilder().SetTake(50);

			var req = builder.Build();
			Assert.Equal(HttpMethod.Get, req.Method);
			Assert.Equal(_apiKey, req.Headers.GetValues("APIKey").FirstOrDefault());
			Assert.Equal(_guid, req.Headers.GetValues("guid").FirstOrDefault());
			Assert.Equal("https://ade6cf3c-api.agilitycms.cloud/en-us/list/posts?skip=0&take=50&contentLinkDepth=1", req.RequestUri.AbsoluteUri);
		}

		[Fact]
		public void Take51BuilderTest() {
			var builder = GetDefaultConfiguredBuilder().SetTake(50);

			var req = builder.Build();
			Assert.Equal(HttpMethod.Get, req.Method);
			Assert.Equal(_apiKey, req.Headers.GetValues("APIKey").FirstOrDefault());
			Assert.Equal(_guid, req.Headers.GetValues("guid").FirstOrDefault());
			Assert.Equal("https://ade6cf3c-api.agilitycms.cloud/en-us/list/posts?skip=0&take=50&contentLinkDepth=1", req.RequestUri.AbsoluteUri);
		}

		[Fact]
		public void ContentLinkDepth2BuilderTest() {
			var builder = GetDefaultConfiguredBuilder().SetContentLinkDepth(2);

			var req = builder.Build();
			Assert.Equal(HttpMethod.Get, req.Method);
			Assert.Equal(_apiKey, req.Headers.GetValues("APIKey").FirstOrDefault());
			Assert.Equal(_guid, req.Headers.GetValues("guid").FirstOrDefault());
			Assert.Equal("https://ade6cf3c-api.agilitycms.cloud/en-us/list/posts?skip=0&take=10&contentLinkDepth=2", req.RequestUri.AbsoluteUri);
		}

		[Fact]
		public void FilteredBuilderTest() {
			var titleProp = "title";
			var titleVal = "test title";
			var filter = new AgilityContentFilterBuilder()
							  .AddFilter(titleProp, titleVal);

			var builder = GetDefaultConfiguredBuilder()
				.SetFilter(filter);

			var req = builder.Build();
			Assert.Equal(HttpMethod.Get, req.Method);
			Assert.Equal(_apiKey, req.Headers.GetValues("APIKey").FirstOrDefault());
			Assert.Equal(_guid, req.Headers.GetValues("guid").FirstOrDefault());
			Assert.Equal("https://ade6cf3c-api.agilitycms.cloud/en-us/list/posts?skip=0&take=10&contentLinkDepth=1&filter=" + filter.Build(), req.RequestUri.AbsoluteUri);
		}


		AgilityContentListRequestBuilder GetDefaultConfiguredBuilder() =>
			GetDefaultBuilder().SetLanguage("en-us").SetReferenceName("posts");

		AgilityContentListRequestBuilder GetDefaultBuilder() =>
			new AgilityContentListRequestBuilder(_apiKey, _guid);
	}
}
