using System;

namespace Crawl.WebAPI.Common.Contract.Image
{
	public class ImageResponseData
	{
		public Guid ImageAppKey { get; set; }
		public int Version { get; set; }
		public string ImageWebSrc { get; set; }
		public string ImageUrl { get; set; }
	}
}