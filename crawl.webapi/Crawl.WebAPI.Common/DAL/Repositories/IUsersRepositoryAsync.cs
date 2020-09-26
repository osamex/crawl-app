using System.Threading.Tasks;
using Crawl.WebAPI.Common.Domain.Entities;

namespace Crawl.WebAPI.Common.DAL.Repositories
{
	public interface IUsersRepositoryAsync : IBaseRepositoryAsync<UserEntity>
	{
		Task<UserEntity> GetUser(string email);
	}
}