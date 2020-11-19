using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Web.Models;


namespace zero.Web.Controllers
{
  public abstract class BackofficeCollectionController<TEntity, TCollection> : BackofficeController 
    where TEntity : IZeroEntity 
    where TCollection : ICollectionBase<TEntity>
  {
    protected TCollection Collection { get; private set; }

    protected Func<IRavenQueryable<TEntity>, IRavenQueryable<TEntity>> DefaultQuery { get; set; }

    protected Action<TEntity, PreviewModel> PreviewTransform { get; set; }


    public BackofficeCollectionController(TCollection collection)
    {
      Collection = collection;
    }

    public override void OnScopeChanged(string scope)
    {
      Collection.ApplyScope(scope);
    }


    public virtual async Task<EditModel<TEntity>> GetById([FromQuery] string id) => Edit(await Collection.GetById(id));


    public virtual EditModel<TEntity> GetEmpty([FromServices] TEntity blueprint) => Edit(blueprint);


    public virtual async Task<ListResult<TEntity>> GetByQuery([FromQuery] ListQuery<TEntity> query)
    {
      query.SearchSelector = model => model.Name;
      IRavenQueryable<TEntity> ravenQuery = Collection.Query;
      if (DefaultQuery != null)
      {
        ravenQuery = DefaultQuery(ravenQuery);
      }

      return await ravenQuery.ToQueriedListAsync(query);
    }


    public virtual async Task<IEnumerable<SelectModel>> GetForPicker() => await SelectList(Collection.Stream());


    public virtual async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids) => Previews(await Collection.GetByIds(ids.ToArray()), PreviewTransform);


    public virtual async Task<EntityResult<TEntity>> Save([FromBody] TEntity model) => await Collection.Save(model);


    public virtual async Task<EntityResult<TEntity>> Delete([FromQuery] string id) => await Collection.DeleteById(id);
  }
}
