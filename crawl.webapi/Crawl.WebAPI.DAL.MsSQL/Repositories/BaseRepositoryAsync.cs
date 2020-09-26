using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Crawl.WebAPI.Common.Contract;
using Crawl.WebAPI.Common.Contract.QueryFilter;
using Crawl.WebAPI.Common.DAL.Repositories;
using Crawl.WebAPI.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Crawl.WebAPI.DAL.MsSQL.Repositories
{
  public class BaseRepositoryAsync<TEntity> : IBaseRepositoryAsync<TEntity> where TEntity : BaseEntity, new()
  {
    private readonly DataBaseContext _context;
    private readonly DbSet<TEntity> _dbSet;
    private readonly ILogger _logger;
    private readonly string _entityName;
    private readonly IMapper _mapper;

    protected BaseRepositoryAsync(DataBaseContext context, ILogger logger, IMapper mapper)
    {
      _context = context;
      _dbSet = _context.Set<TEntity>();
      _logger = logger;
      _entityName = typeof(TEntity).Name;
      _mapper = mapper;
    }

    public virtual void Dispose()
    {
      _context?.Dispose();
    }

    public virtual async Task<long> Add(TEntity entity)
    {
      var stopper = new Stopwatch();
      stopper.Start();

      try
      {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.DbKey;
      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute Add [{@_entityName}] with data: {@entity}", _entityName, entity);
        return -1;
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("Add [{@_entityName}] to DB total time: {1} [ms]", _entityName, stopper.Elapsed.TotalMilliseconds);
      }
    }

    public virtual async Task<ICollection<long>> AddRange(ICollection<TEntity> entities)
    {
      var stopper = new Stopwatch();
      stopper.Start();
      try
      {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        return entities.Where(x => x.DbKey > 0 && x.AppKey != Guid.Empty).Select(x => x.DbKey).ToList();
      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute AddRange [{@_entityName}] with data: {@entities}", _entityName, entities);
        return new List<long>();
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("Add {0} [{@_entityName}] on DB total time: {1} [ms]", entities.Count, _entityName, stopper.Elapsed.TotalMilliseconds);
      }
    }

    public virtual async Task<IDictionary<long, bool>> DeleteRange(ICollection<long> ids)
    {
      var stopper = new Stopwatch();
      stopper.Start();
      var returnDictionary = new Dictionary<long, bool>();
      try
      {
        var entities = _dbSet.AsNoTracking().Where(x => ids.Contains(x.DbKey));
        if (ids.Count == entities.Count())
        {
          _dbSet.RemoveRange(entities);
          await _context.SaveChangesAsync();

          foreach (var id in ids)
          {
            returnDictionary.Add(id, _dbSet.AsNoTracking().SingleOrDefault(x => x.DbKey == id) == null);
          }
        }

        return returnDictionary;
      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute DeleteRange [{@_entityName}] deleting list: {@returnDictionary} ", _entityName, returnDictionary);
        return returnDictionary;
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("Delete {0} [{@_entityName}] on DB total time: {2} [ms] ", ids.Count, _entityName, stopper.Elapsed.TotalMilliseconds);
      }
    }

    public virtual async Task<bool> Delete(Guid appKey)
    {
      var stopper = new Stopwatch();
      stopper.Start();

      try
      {
        var entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.AppKey == appKey);
        if (entity != null)
        {
          _dbSet.Remove(entity);
          await _context.SaveChangesAsync();
          return _dbSet.AsNoTracking().SingleOrDefault(e => e.AppKey == appKey) == null;
        }

        return false;
      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute Delete [{@_entityName}] : {@appKey} ", _entityName, appKey);
        return false;
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("Delete [{@_entityName}] to DB total time: {1} [ms] ", _entityName, stopper.Elapsed.TotalMilliseconds);
      }
    }

    public virtual async Task<IDictionary<Guid, bool>> DeleteRange(ICollection<Guid> appKeys)
    {
      var stopper = new Stopwatch();
      stopper.Start();
      var returnDictionary = new Dictionary<Guid, bool>();
      try
      {
        var entities = _dbSet.AsNoTracking().Where(x => appKeys.Contains(x.AppKey));
        if (appKeys.Count == entities.Count())
        {
          _dbSet.RemoveRange(entities);
          await _context.SaveChangesAsync();

          foreach (var id in appKeys)
          {
            returnDictionary.Add(id, _dbSet.AsNoTracking().SingleOrDefault(x => x.AppKey == id) == null);
          }
        }

        return returnDictionary;
      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute DeleteRange [{@_entityName}] deleting list: {@returnDictionary} ", _entityName, returnDictionary);
        return returnDictionary;
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("Delete {0} [{@_entityName}] on DB total time: {2} [ms] ", appKeys.Count, _entityName, stopper.Elapsed.TotalMilliseconds);
      }
    }

    public virtual async Task<bool> Update(TEntity entity)
    {
      var stopper = new Stopwatch();
      stopper.Start();

      try
      {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        _context.Entry(entity).State = EntityState.Detached;
        return true;
      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute Update [{@_entityName}] with data: {@entity} ", _entityName, entity);
        return false;
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("Update [{@_entityName}] to DB total time: {1} [ms] ", _entityName, stopper.Elapsed.TotalMilliseconds);
      }
    }

    public virtual async Task<IDictionary<long, bool>> UpdateRange(ICollection<TEntity> entities)
    {
      var stopper = new Stopwatch();
      stopper.Start();
      var returnDictionary = new Dictionary<long, bool>();
      try
      {
        foreach (var entity in entities)
        {
          _context.Entry(entity).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();

        foreach (var entity in entities)
        {
          _context.Entry(entity).State = EntityState.Detached;
          returnDictionary.Add(entity.DbKey, true);
        }

        return returnDictionary;
      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute UpdateRange [{@_entityName}] deleting list: {@returnDictionary} ", _entityName, returnDictionary);
        return returnDictionary;
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("UpdateRange {0} [{@_entityName}] on DB total time: {2} [ms] ", entities.Count, _entityName, stopper.Elapsed.TotalMilliseconds);
      }
    }

    public virtual async Task<TResponseData> GetByAppKey<TResponseData>(Guid appKey) where TResponseData : class
    {
      return _mapper.Map<TEntity, TResponseData>(await GetByAppKey(appKey));
    }

    public virtual Task<TEntity> GetByAppKey(Guid appKey)
    {
      var stopper = new Stopwatch();
      stopper.Start();
      try
      {
        return _dbSet.AsNoTracking().SingleOrDefaultAsync(s => s.AppKey == appKey);
      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute GetByAppKey");
        return default;
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("GetByAppKey total time: {0} | appKey: {1}", stopper.Elapsed.TotalMilliseconds, appKey);
      }
    }

    public virtual Task<PaginatedListResponseData<TResponseData>> GetFiltered<TResponseData>(Expression<Func<TEntity, bool>> expression = null, QueryFilterRequest filterRequest = null) where TResponseData : class
    {
      var stopper = new Stopwatch();
      stopper.Start();

      try
      {
        filterRequest ??= new QueryFilterRequest();
        var source = expression == null ? _dbSet.AsNoTracking() : _dbSet.AsNoTracking().Where(expression);
        return Task.FromResult(PaginatedListResponseData<TResponseData>.Create(source, filterRequest, _mapper));

      }
      catch (Exception e)
      {
        _logger.Error(e, "Error when execute GetFiltered | Expression: {0} ", expression);
        return default;
      }
      finally
      {
        stopper.Stop();
        _logger.Debug("GetFiltered total time: {0} | Expression: {1}", stopper.Elapsed.TotalMilliseconds, expression);
      }
    }

    public virtual async Task<TResponseData> SingleOrDefault<TResponseData>(Expression<Func<TEntity, bool>> expression) where TResponseData : class
    {
      return _mapper.Map<TEntity, TResponseData>(await SingleOrDefault(expression));
    }

    public virtual Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> expression)
    {
      var stopper = new Stopwatch();
      stopper.Start();

      try
      {
        if (expression == null)
        {
          throw new InvalidExpressionException("No expression defined!");
        }

        return _dbSet.AsNoTracking().SingleOrDefaultAsync(expression);
      }
      catch (Exception e)
      {
        if (expression != null)
        {
          _logger.Error(e, "Error when execute SingleOrDefault | Expression: {0} ", expression);
        }
        else
        {
          _logger.Error(e, "Error when execute SingleOrDefault | Expression: IS NULL!");
        }
      }
      finally
      {
        stopper.Stop();
        if (expression != null)
        {
          _logger.Debug("SingleOrDefault total time: {0} | Expression: {1}", stopper.Elapsed.TotalMilliseconds, expression);
        }
        else
        {
          _logger.Debug("SingleOrDefault total time: {0} | Expression: IS NULL!", stopper.Elapsed.TotalMilliseconds);
        }
      }

      return default;
    }

    public virtual async Task<TResponseData> FirstOrDefault<TResponseData>(Expression<Func<TEntity, bool>> expression) where TResponseData : class
    {
      return _mapper.Map<TEntity, TResponseData>(await FirstOrDefault(expression));
    }

    public virtual Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression)
    {
      var stopper = new Stopwatch();
      stopper.Start();

      try
      {
        if (expression == null)
        {
          throw new InvalidExpressionException("No expression defined!");
        }

        return _dbSet.AsNoTracking().FirstOrDefaultAsync(expression);
      }
      catch (Exception e)
      {
        if (expression != null)
        {
          _logger.Error(e, "Error when execute FirstOrDefault | Expression: {0} ", expression);
        }
        else
        {
          _logger.Error(e, "Error when execute FirstOrDefault | Expression: IS NULL!");
        }
      }
      finally
      {
        stopper.Stop();
        if (expression != null)
        {
          _logger.Debug("FirstOrDefault total time: {0} | Expression: {1}", stopper.Elapsed.TotalMilliseconds, expression);
        }
        else
        {
          _logger.Debug("FirstOrDefault total time: {0} | Expression: IS NULL!", stopper.Elapsed.TotalMilliseconds);
        }
      }

      return default;
    }
  }
}