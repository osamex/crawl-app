using AutoMapper;
using Crawl.WebAPI.Common.DAL.Repositories;
using Crawl.WebAPI.Common.Domain.Entities;
using Serilog;

namespace Crawl.WebAPI.DAL.MsSQL.Repositories
{
	public class SitesRepositoryAsync : BaseRepositoryAsync<SiteEntity>, ISitesRepositoryAsync
	{
		public SitesRepositoryAsync(DataBaseContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
		{
		}
	}
}