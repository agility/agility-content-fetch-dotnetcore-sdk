using System.Net;
using Xunit;
namespace Agility.Fetch.Tests {
	public class AgilityContentFilterBuilderTests {		
		[Theory]
		[InlineData(FilterPredicate.Equal, "eq")]
		[InlineData(FilterPredicate.NotEqual, "ne")]
		[InlineData(FilterPredicate.LessThanOrEqual, "lte")]
		[InlineData(FilterPredicate.LessThan, "lt")]
		[InlineData(FilterPredicate.GreaterThanOrEqual, "gte")]
		[InlineData(FilterPredicate.GreaterThan, "gt")]
		[InlineData(FilterPredicate.Like, "like")]
		public void SingleTest(FilterPredicate pred, string predStr) {
			var titleProp = "title";
			var titleVal = "test title";
			var builder = new AgilityContentFilterBuilder()
				.AddFilter(titleProp, titleVal, pred);

			var filterStr = builder.Build();

			var expected = WebUtility.UrlEncode($"{titleProp}[{predStr}]{titleVal}");
			Assert.Equal(expected, filterStr);
		}

		[Theory]
		[InlineData(FilterPredicate.Equal, "eq")]
		[InlineData(FilterPredicate.NotEqual, "ne")]
		[InlineData(FilterPredicate.LessThanOrEqual, "lte")]
		[InlineData(FilterPredicate.LessThan, "lt")]
		[InlineData(FilterPredicate.GreaterThanOrEqual, "gte")]
		[InlineData(FilterPredicate.GreaterThan, "gt")]
		[InlineData(FilterPredicate.Like, "like")]
		public void MultiTest(FilterPredicate pred, string predStr) {
			var titleProp = "title";
			var titleVal = "test title";

			var nameProp = "name";
			var nameVal = "Agility";

			var builder = new AgilityContentFilterBuilder()
				.AddFilter(titleProp, titleVal, pred)
				.AddFilter(nameProp, nameVal, pred);

			var filterStr = builder.Build();

			var expected = WebUtility.UrlEncode($"{titleProp}[{predStr}]{titleVal} AND {nameProp}[{predStr}]{nameVal}");
			Assert.Equal(expected, filterStr);
		}

		[Theory]
		[InlineData(FilterPredicate.Equal, "eq")]
		[InlineData(FilterPredicate.NotEqual, "ne")]
		[InlineData(FilterPredicate.LessThanOrEqual, "lte")]
		[InlineData(FilterPredicate.LessThan, "lt")]
		[InlineData(FilterPredicate.GreaterThanOrEqual, "gte")]
		[InlineData(FilterPredicate.GreaterThan, "gt")]
		[InlineData(FilterPredicate.Like, "like")]
		public void MultiOrTest(FilterPredicate pred, string predStr) {
			var titleProp = "title";
			var titleVal = "test title";

			var nameProp = "name";
			var nameVal = "Agility";

			var builder = new AgilityContentFilterBuilder()
				.AddFilter(titleProp, titleVal, pred, FilterExpression.Or)
				.AddFilter(nameProp, nameVal, pred, FilterExpression.Or);

			var filterStr = builder.Build();

			var expected = WebUtility.UrlEncode($"{titleProp}[{predStr}]{titleVal} OR {nameProp}[{predStr}]{nameVal}");
			Assert.Equal(expected, filterStr);
		}
	}
}
