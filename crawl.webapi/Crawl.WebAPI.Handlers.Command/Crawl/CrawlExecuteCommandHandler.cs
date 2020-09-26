using System.Threading;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Command.Crawl;
using MediatR;

namespace Crawl.WebAPI.Handlers.Command.Crawl
{
	public class CrawlExecuteCommandHandler : INotificationHandler<CrawlExecuteCommand>
	{
		public Task Handle(CrawlExecuteCommand command, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}