using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Indexes;
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
    where TEntity : ZeroEntity 
    where TCollection : ICollectionBase<TEntity>
  {
    protected TCollection Collection { get; private set; }

    [Obsolete]
    protected Func<IRavenQueryable<TEntity>, IRavenQueryable<TEntity>> DefaultQuery { get; set; }

    protected Action<TEntity, PreviewModel> PreviewTransform { get; set; }

    protected Action<TEntity, SelectModel> PickerTransform { get; set; }


    public BackofficeCollectionController(TCollection collection)
    {
      Collection = collection;
    }

    public override void OnScopeChanged(string scope)
    {
      Collection.ApplyScope(scope);
    }


    public virtual async Task<EditModel<TEntity>> GetById([FromQuery] string id, [FromQuery] string changeVector = null) => Edit(changeVector.IsNullOrEmpty() ? await Collection.GetById(id) : await Collection.GetRevision(changeVector));


    public virtual async Task<Dictionary<string, TEntity>> GetByIds([FromQuery] string[] ids) => await Collection.GetByIds(ids);


    public virtual EditModel<TEntity> GetEmpty([FromServices] TEntity blueprint) => Edit(blueprint);


    public virtual async Task<ListResult<TEntity>> GetByQuery([FromQuery] ListBackofficeQuery<TEntity> query)
    {
      return await Collection.GetByQuery(query);
    }


    public virtual async Task<ListResult<Revision>> GetRevisions([FromQuery] string id, [FromQuery] ListBackofficeQuery<TEntity> query)
    {
      return await Collection.GetRevisions(id, query.Page, query.PageSize);
    }


    public virtual async Task<IEnumerable<SelectModel>> GetForPicker() => await SelectList(Collection.Stream(), PickerTransform);


    public virtual async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids) => Previews(await Collection.GetByIds(ids.ToArray()), PreviewTransform);


    [HttpPost]
    public virtual async Task<EntityResult<TEntity>> Save([FromBody] TEntity model) => await Collection.Save(model);


    [HttpDelete]
    public virtual async Task<EntityResult<TEntity>> Delete([FromQuery] string id) => await Collection.DeleteById(id);
  }
}
