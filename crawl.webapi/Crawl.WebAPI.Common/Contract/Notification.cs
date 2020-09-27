using Crawl.WebAPI.Common.Enums;

namespace Crawl.WebAPI.Common.Contract
{
	public class Notification
	{
		public string Message { get; set; }
		public MessageTypes Type { get; set; }
	}
}