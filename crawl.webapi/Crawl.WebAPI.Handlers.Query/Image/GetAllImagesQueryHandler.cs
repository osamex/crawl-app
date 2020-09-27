using System.Threading;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.Auth;
using Crawl.WebAPI.Common.Contract.Image;
using Crawl.WebAPI.Common.Contract.QueryFilter;
using Crawl.WebAPI.Common.DAL.Repositories;
using Crawl.WebAPI.Common.Query.Auth;
using Crawl.WebAPI.Common.Query.Image;
using MediatR;

namespace Crawl.WebAPI.Handlers.Query.Image
{
	public class GetAllImagesQueryHandler : IRequestHandler<GetAllImagesQuery, PaginatedListResponseData<ImageResponseData>>
	{

		private readonly IImagesRepositoryAsync _imagesRepository;
		private readonly ISitesRepositoryAsync _sitesRepository;

		public GetAllImagesQueryHandler(IImagesRepositoryAsync imagesRepository, ISitesRepositoryAsync sitesRepository)
		{
			_imagesRepository = imagesRepository;
			_sitesRepository = sitesRepository;
		}

		public async Task<PaginatedListResponseData<ImageResponseData>> Handle(GetAllImagesQuery query, CancellationToken cancellationToken)
		{
			var site = await _sitesRepository.FirstOrDefault(x => x.Url.Contains(query.Request.RequestData));
			if (site != null)
			{
				return await _imagesRepository.GetFiltered<ImageResponseData>(w => w.SiteDbKey == site.DbKey);
			}
			return new PaginatedListResponseData<ImageResponseData>();
		}
	}
}