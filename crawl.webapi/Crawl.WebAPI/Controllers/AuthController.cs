using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crawl.WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class AuthController : Controller
	{
		[HttpGet("IsAlive")]
		[AllowAnonymous]
		public IActionResult IsAlive()
		{
			try
			{
				return Ok("Is alive!");
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}