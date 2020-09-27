using AutoMapper;
using Crawl.WebAPI.Common.DAL.Repositories;
using Crawl.WebAPI.Common.Domain.Entities;
using Serilog;

namespace Crawl.WebAPI.DAL.MsSQL.Repositories
{
	public class ImagesRepositoryAsync : BaseRepositoryAsync<ImageEntity>, IImagesRepositoryAsync
	{
		public ImagesRepositoryAsync(DataBaseContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
		{
		}
	}
}