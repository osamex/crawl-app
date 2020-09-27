namespace Crawl.WebAPI.Common.Domain.Entities
{
	public class ImageEntity : BaseEntity
	{
		public int Version { get; set; }
		public string ImageUrl { get; set; }
		public byte[] Image { get; set; }
		public long SiteDbKey { get; set; }
		public SiteEntity Site { get; set; }
	}
}