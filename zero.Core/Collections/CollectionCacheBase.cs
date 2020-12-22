using FluentValidation;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Collections
{
  public abstract class CollectionCacheBase<T> : CollectionBase<T>, ICollectionBase<T>, IDisposable where T : IZeroEntity
  {
    public CollectionCacheBase(IZeroContext context, ICollectionInterceptorHandler interceptorHandler, IValidator<T> validator = null) : base(context, interceptorHandler, validator) { }


    protected List<T> Items { get; set; }


    protected async Task Preload()
    {
      if (Items == null || !Items.Any())
      {
        Items = await Session.Query<T>().ToListAsync();
      }
    }


    /// <inheritdoc />
    public override void ApplyScope(string scope)
    {
      Items = null;
      base.ApplyScope(scope);
    }


    /// <inheritdoc />
    public override async Task<T> GetById(string id)
    {
      await Preload();

      if (id.IsNullOrWhiteSpace())
      {
        return default;
      }

      return Items.FirstOrDefault(x => x.Id == id);
    }


    /// <inheritdoc />
    public override async Task<Dictionary<string, T>> GetByIds(params string[] ids)
    {
      await Preload();

      Dictionary<string, T> models = Items.Where(x => ids.Contains(x.Id)).ToDictionary(x => x.Id, x => x);
      Dictionary<string, T> result = new Dictionary<string, T>();

      foreach (string id in ids)
      {
        models.TryGetValue(id, out T model);
        result.Add(id, model);
      }

      return result;
    }


    /// <inheritdoc />
    public override async Task<ListResult<T>> GetByQuery(ListQuery<T> query)
    {
      return await Session.Query<T>().OrderByDescending(x => x.CreatedDate).ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public override async Task<List<T>> GetAll()
    {
      await Preload();
      return Items;
    }


    /// <inheritdoc />
    public override async Task<EntityResult<T>> Save(T model)
    {
      EntityResult<T> result = await base.Save(model);
      if (result.IsSuccess)
      {
        Items = null;
      }
      return result;
    }


    /// <inheritdoc />
    public override async Task<EntityResult<T>> DeleteById(string id)
    {
      EntityResult<T> result = await base.DeleteById(id);
      if (result.IsSuccess)
      {
        Items = null;
      }
      return result;
    }

    /// <inheritdoc />
    public override async Task<int> DeleteByIds(params string[] ids)
    {
      int successCount = await base.DeleteByIds(ids);
      if (successCount > 0)
      {
        Items = null;
      }
      return successCount;
    }


    /// <inheritdoc />
    public override async Task<EntityResult<T>> Purge(string querySuffix = null, Parameters parameters = null)
    {
      EntityResult<T> result = await base.Purge(querySuffix, parameters);
      if (result.IsSuccess)
      {
        Items = null;
      }
      return result;
    }
  }
}
