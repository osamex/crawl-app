using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.Image;
using MediatR;

namespace Crawl.WebAPI.Common.Query.Image
{
	public class GetAllImagesQuery : IRequest<PaginatedListResponseData<ImageResponseData>>
	{
		public Request<string> Request { get; set; }
	}
}