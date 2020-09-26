using System.Threading;
using System.Threading.Tasks;

namespace Crawl.WebAPI.Common.DAL
{
	public interface IDataContext
	{
		bool Migrate();
		bool Seed();
		int SaveChanges();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		bool? ForcedExecuteSeed { get; }
	}
}