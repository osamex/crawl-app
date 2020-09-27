using System;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crawl.WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[AllowAnonymous]
	public class NotificationController : Controller
	{

		private readonly NotificationHub _hub;

		public NotificationController(NotificationHub hub)
		{
			_hub = hub;
		}

		[HttpPost("Send")]
		[AllowAnonymous]
		public async Task<IActionResult> Send([FromBody] Notification notification)
		{
			try
			{
				await _hub.SendNotification(notification);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}