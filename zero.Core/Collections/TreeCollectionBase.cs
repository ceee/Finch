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
  public abstract class TreeCollectionBase<T> : CollectionBase<T>, ITreeCollectionBase<T>, IDisposable where T : IZeroEntity, ITreeEntity
  {
    public TreeCollectionBase(IZeroContext context, ICollectionInterceptorHandler interceptorHandler, IValidator<T> validator = null) : base(context, interceptorHandler, validator) { }


    ///// <inheritdoc />
    //public async Task<List<T>> GetHierarchy(string id)
    //{
    //  Categories_ByHierarchy.Result result = await Session.Query<Categories_ByHierarchy.Result, Categories_ByHierarchy>()
    //    .ProjectInto<Categories_ByHierarchy.Result>()
    //    .Include<Categories_ByHierarchy.Result, ICategory>(x => x.Path.Select(p => p.Id))
    //    .FirstOrDefaultAsync(x => x.Id == id);

    //  if (result == null)
    //  {
    //    return new();
    //  }

    //  return (await Session.LoadAsync<T>(result.Path.Select(x => x.Id))).Select(x => x.Value).ToList();
    //}
  }


  public interface ITreeCollectionBase<T> : ICollectionBase<T> where T : IZeroEntity, ITreeEntity
  {
    ///// <summary>
    ///// Get the tree hierarchy path for this entity
    ///// </summary>
    //Task<List<T>> GetHierarchy(string id);

    ///// <summary>
    ///// Update sorting of entities on a specific level
    ///// </summary>
    //Task<EntityResult<List<T>>> SaveSorting(string[] sortedIds);

    ///// <summary>
    ///// Move an entity to a new parent
    ///// </summary>
    //Task<EntityResult<T>> Move(string id, string parentId);

    ///// <summary>
    ///// Copies an entity to a new location
    ///// </summary>
    //Task<EntityResult<T>> Copy(string id, string destinationId, bool includeDescendants = false);
  }
}
