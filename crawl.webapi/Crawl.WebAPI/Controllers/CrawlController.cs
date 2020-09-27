using System;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Command.Crawl;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.Crawl;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crawl.WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class CrawlController : Controller
	{
		private readonly IMediator _mediator;

		public CrawlController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("Execute")]
		public async Task<IActionResult> Execute([FromBody] Request<CrawlRequestData> request)
		{
			try
			{
				await _mediator.Publish(new CrawlExecuteCommand { Request = request }).ConfigureAwait(false);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}