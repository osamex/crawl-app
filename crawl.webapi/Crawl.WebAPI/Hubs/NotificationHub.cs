using System;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Contract;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Crawl.WebAPI.Hubs
{
	public class NotificationHub : Hub
	{
		private readonly ILogger _logger;

		public NotificationHub(ILogger logger)
		{
			_logger = logger;
		}

		public override Task OnConnectedAsync()
		{
			_logger.Debug($"Connect: {Context.ConnectionId}");
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			_logger.Warning($"Disconnect: {Context.ConnectionId}");
			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendNotification(Notification notification)
		{
			if (Clients != null)
			{
				await Clients.All.SendAsync("Notify", notification);
			}
		}
	}
}