using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Entities;


namespace zero.Backoffice;

public abstract class _ZeroBackofficeCollectionController<TEntity, TCollection> : ZeroBackofficeController
  where TEntity : ZeroIdEntity, new()
  where TCollection : ICollectionOperations<TEntity>
{
  protected TCollection Collection { get; private set; }

  [Obsolete]
  protected Func<IRavenQueryable<TEntity>, IRavenQueryable<TEntity>> DefaultQuery { get; set; }

  protected Action<TEntity, PreviewModel> PreviewTransform { get; set; }

  protected Action<TEntity, SelectModel> PickerTransform { get; set; }


  public ZeroBackofficeCollectionController(TCollection collection)
  {
    Collection = collection;
  }

  public override void OnScopeChanged(string scope)
  {
    //Collection.ApplyScope(scope);
  }


  public virtual async Task<EditModel<TEntity>> GetById([FromQuery] string id, [FromQuery] string changeVector = null) => Edit(await Collection.Load(id, changeVector));


  public virtual async Task<Dictionary<string, TEntity>> GetByIds([FromQuery] string[] ids) => await Collection.Load(ids);


  public virtual async Task<EditModel<TEntity>> GetEmpty() => Edit(await Collection.Empty());


  public virtual async Task<Paged<TEntity>> GetByQuery([FromQuery] ListBackofficeQuery<TEntity> query)
  {
    return await Collection.Load(query);
  }


  public virtual async Task<Paged<Revision>> GetRevisions([FromQuery] string id, [FromQuery] ListBackofficeQuery<TEntity> query)
  {
    return null; // TODO
    //return await Collection.GetRevisions(id, query.Page, query.PageSize);
  }


  public virtual async Task<IEnumerable<SelectModel>> GetForPicker() => await SelectList(Collection.Stream(), PickerTransform);


  public virtual async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids) => Previews(await Collection.Load(ids), PreviewTransform);


  [HttpPost]
  public virtual async Task<EntityResult<TEntity>> Save([FromBody] TEntity model) => await Collection.Save(model);


  [HttpDelete]
  public virtual async Task<EntityResult<TEntity>> Delete([FromQuery] string id) => await Collection.Delete(id);
}