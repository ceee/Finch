using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Collections
{
  public abstract class FixedCollectionBase<T> : CollectionBase<T>, IFixedCollectionBase<T>, IDisposable where T : IZeroTypedEntity
  {
    public FixedCollectionBase(IZeroContext context, ICollectionInterceptorHandler interceptorHandler = null, IValidator<T> validator = null) : base(context, interceptorHandler, validator) { }


    protected abstract IEnumerable<OptionsType> GetDefinedTypes();


    /// <inheritdoc />
    public virtual async Task<TSpecific> GetByType<TSpecific>() where TSpecific : T, new()
    {
      OptionsType type = GetDefinedTypes().FirstOrDefault(x => x.ContentType == typeof(TSpecific));
      return await GetEntity<TSpecific>(type);
    }


    /// <inheritdoc />
    public virtual async Task<TSpecific> GetByType<TSpecific>(string alias) where TSpecific : T, new()
    {
      OptionsType type = GetDefinedTypes().FirstOrDefault(x => x.ContentType == typeof(TSpecific) && x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
      return await GetEntity<TSpecific>(type);
    }


    /// <inheritdoc />
    public virtual async Task<T> GetByAlias(string alias)
    {
      OptionsType type = GetDefinedTypes().FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
      return type != null ? await Session.Query<T>().FirstOrDefaultAsync(x => x.TypeAlias == type.Alias) : default;
    }


    ///// <inheritdoc />
    //public override async Task<ListResult<T>> GetByQuery(ListQuery<T> query)
    //{
    //  ListResult<T> list = await Session.Query<T>().OrderByDescending(x => x.CreatedDate).ToQueriedListAsync(query);
    //}


    /// <summary>
    /// Get data from database
    /// </summary>
    async Task<TSpecific> GetEntity<TSpecific>(OptionsType type) where TSpecific : T, new()
    {
      if (type == null)
      {
        return default;
      }

      TSpecific model = await Session.Query<TSpecific>().FirstOrDefaultAsync(x => x.TypeAlias == type.Alias);

      if (model == null && type.IsAutoActivated)
      {
        return new TSpecific();
      }

      return model;
    }
  }


  public interface IFixedCollectionBase<T> : IDisposable where T : IZeroEntity
  {
    /// <summary>
    /// Guid for this instance
    /// </summary>
    Guid Guid { get; }

    /// <summary>
    /// The database to operate on.
    /// Is null by default, which uses the database from the resolved application.
    /// </summary>
    string Database { get; set; }

    /// <summary>
    /// Returns a new document queryable
    /// </summary>
    IRavenQueryable<T> Query { get; }

    /// <summary>
    /// Applies the scope to the service instance
    /// </summary>
    void ApplyScope(string scope);

    /// <summary>
    /// Get an entity by alias
    /// </summary>
    //Task<T> GetByAlias(string alias);
  }
}
