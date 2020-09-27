using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crawl.WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[AllowAnonymous]
	public class TestController : Controller
	{
		[AllowAnonymous]
		public IActionResult Get()
		{
			try
			{
				var response = new
				{
					IsAlive = true
				};
				return Ok(response);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
