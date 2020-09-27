using System;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Query.Image;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crawl.WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class ImageController : Controller
	{
		private readonly IMediator _mediator;

		public ImageController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("GetAllImages")]
		public async Task<IActionResult> GetAllImages([FromBody] Request<string> request)
		{
			try
			{
				var response = await _mediator.Send(new GetAllImagesQuery { Request = request });
				return Ok(response);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}