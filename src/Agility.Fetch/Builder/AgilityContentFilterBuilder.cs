using System.Collections.Generic;
using System.Net;

namespace Agility.Fetch {
	public class AgilityContentFilterBuilder {
		readonly ISet<string> _filters = new HashSet<string>();

		public string Build() {
			var filterStr = string.Join(" ", _filters);
			int start = string.Equals("AND", filterStr.Substring(0, 3)) ?
				4 :
				3;

			return WebUtility.UrlEncode(filterStr.Substring(start));
		}

		public AgilityContentFilterBuilder AddFilter(
			string property,
			string value,
			FilterPredicate predicate = FilterPredicate.Equal,
			FilterExpression expression = FilterExpression.And) {
			string pred;

			switch (predicate) {
				case FilterPredicate.Like:
					pred = "like";
					break;
				case FilterPredicate.LessThan:
					pred = "lt";
					break;
				case FilterPredicate.LessThanOrEqual:
					pred = "lte";
					break;
				case FilterPredicate.GreaterThan:
					pred = "gt";
					break;
				case FilterPredicate.GreaterThanOrEqual:
					pred = "gte";
					break;
				case FilterPredicate.NotEqual:
					pred = "ne";
					break;
				case FilterPredicate.Equal:
				default:
					pred = "eq";
					break;
			}

			string expr = expression == FilterExpression.And ? "AND" : "OR";
			_filters.Add($"{expr} {property}[{pred}]{value}");
			return this;
		}

	}
}
