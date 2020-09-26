using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.Crawl;
using MediatR;

namespace Crawl.WebAPI.Common.Command.Crawl
{
	public class CrawlExecuteCommand : INotification
	{
		public Request<CrawlRequestData> Request { get; set; }
	}
}