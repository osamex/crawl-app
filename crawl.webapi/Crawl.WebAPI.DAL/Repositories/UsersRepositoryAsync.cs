using System;
using System.Threading.Tasks;
using AutoMapper;
using Crawl.WebAPI.Common.DAL.Repositories;
using Crawl.WebAPI.Common.Domain.Entities;
using Serilog;

namespace Crawl.WebAPI.DAL.Repositories
{
	public class UsersRepositoryAsync : BaseRepositoryAsync<UserEntity>, IUsersRepositoryAsync
	{
		public UsersRepositoryAsync(DataBaseContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
		{
		}

		public Task<UserEntity> GetUser(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				throw new Exception("Email is null or empty");
			}

			return SingleOrDefault(s => s.EMail == email);
		}
	}
}