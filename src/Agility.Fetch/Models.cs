using System;
using System.Collections.Generic;

namespace Agility.Fetch {
	public enum FetchMode {
		Staging = 1,
		Live = 2
	}

	public enum ItemState {
		Staging = 1,
		Published = 2,
		Deleted = 3,
		Approved = 4,
		AwaitingApproval = 5,
		Declined = 6,
		Unpublished = 7
	}

	public enum FilterExpression {
		Or,	
		And
	}

	public enum FilterPredicate {
		Like,		
		LessThan,
		LessThanOrEqual,
		GreaterThan,
		GreaterThanOrEqual,
		NotEqual,
		Equal
	}

	public class ContentList<T> {
		/// <summary>
		/// The paginated array of items returned by the request. 
		/// Default *take* is **10** unless otherwise specified (Maximum allowed it **50**)
		/// </summary>
		public IList<ContentItem<T>> Items { get; set; }

		/// <summary>
		/// The total amount of items in the list. Use this along with *skip* and *take* for pagination.
		/// </summary>
		public int TotalCount { get; set; }
	}

	public class ContentItem<T> {
		/// <summary>
		/// The unique ID of the content item in this language.
		/// </summary>
		public int ContentID { get; set; }

		/// <summary>
		/// The system properties of the content item.
		/// </summary>
		public SystemProperties Properties { get; set; } = new SystemProperties();

		/// <summary>
		/// A dictionary of the fields and the values of the content item.
		/// </summary>
		public T Fields { get; set; }

		/// <summary>
		/// Any SEO related fields for the content item. This is only returned for Dynamic Page Items.
		/// </summary>
		public Seo Seo { get; set; }
	}

	public class Seo {		
		public string MetaDescription { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaHTML { get; set; }
		public bool MenuVisible { get; set; }
		public bool SitemapVisible { get; set; }
	}

	public class SystemProperties {
		/// <summary>
		/// The **state** of this content item. 
		/// **1** = *Staging*, 
		/// **2** = *Published*, 
		/// **3** = *Deleted*, 
		/// **4** = *Approved*, 
		/// **5** = *AwaitingApproval*, 
		/// **6** = *Declined*, 
		/// **7** = *Unpublished*
		/// </summary>
		public ItemState State { get; set; }

		/// <summary>
		/// The date/time this item was created.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// The date/time this item was last modified.
		/// </summary>
		public DateTime Modified { get; set; }

		/// <summary>
		/// The unique versionID of this content item.
		/// </summary>
		public int VersionID { get; set; }
	}
}
