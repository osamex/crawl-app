using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.QueryFilter;
using Crawl.WebAPI.Common.Domain.Entities;

namespace Crawl.WebAPI.Common.DAL.Repositories
{
	public interface IBaseRepositoryAsync<TEntity> : IDisposable where TEntity : BaseEntity, new()
	{
		Task<long> Add(TEntity entity);
		Task<ICollection<long>> AddRange(ICollection<TEntity> entities);
		Task<IDictionary<long, bool>> DeleteRange(ICollection<long> ids);
		Task<bool> Delete(Guid appKey);
		Task<IDictionary<Guid, bool>> DeleteRange(ICollection<Guid> appKeys);
		Task<bool> Update(TEntity entity);
		Task<IDictionary<long, bool>> UpdateRange(ICollection<TEntity> entities);
		Task<TResponseData> GetByAppKey<TResponseData>(Guid appKey) where TResponseData : class;
		Task<TEntity> GetByAppKey(Guid appKey);
		Task<PaginatedListResponseData<TResponseData>> GetFiltered<TResponseData>(Expression<Func<TEntity, bool>> expression = null, QueryFilterRequest filterRequest = null) where TResponseData : class;
		Task<TResponseData> SingleOrDefault<TResponseData>(Expression<Func<TEntity, bool>> expression) where TResponseData : class;
		Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> expression);
		Task<TResponseData> FirstOrDefault<TResponseData>(Expression<Func<TEntity, bool>> expression) where TResponseData : class;
		Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression);
	}
}