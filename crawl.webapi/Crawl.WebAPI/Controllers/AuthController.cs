using System;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crawl.WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class AuthController : Controller
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

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

		[HttpPost("SignIn")]
		[AllowAnonymous]
		public async Task<IActionResult> SignIn([FromBody] Request<AuthRequestData> request)
		{
			try
			{
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}