using System.Collections.Generic;

namespace Crawl.WebAPI.Common.Domain.Entities
{
	public class SiteEntity : BaseEntity
	{
		public SiteEntity()
		{
			Url = string.Empty;
		}

		public string Url { get; set; }
		public IEnumerable<ImageEntity> Images { get; set; }
	}
}