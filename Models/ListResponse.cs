using System;
using System.Collections.Generic;
using System.Text;

namespace Agility.Fetch.Models
{
	public class ListResponse<T>
	{
		public IList<Item<T>> items { get; set; }
		public int totalCount { get; set; }
	}

	public class Item<T>
	{
		public int contentID { get; set; }
		public Properties properties { get; set; } = new Properties();
		public T fields { get; set; }
		public Seo seo { get; set; } = new Seo();
	}

	public class Properties
	{
		public int state { get; set; }
		public DateTime modified { get; set; }
		public int versionID { get; set; }
		public string referenceName { get; set; }
		public string definitionName { get; set; }
		public int itemOrder { get; set; }
	}


	public class Seo
	{
		public string metaDescription { get; set; }
		public object metaKeywords { get; set; }
		public object metaHTML { get; set; }
		public object menuVisible { get; set; }
		public object sitemapVisible { get; set; }
	}


}
