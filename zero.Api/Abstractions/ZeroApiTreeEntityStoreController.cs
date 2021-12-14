using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Indexes;

namespace zero.Api.Abstractions;

public abstract class ZeroApiTreeEntityStoreController<TModel, TStore> : ZeroApiEntityStoreController<TModel, TStore> 
  where TModel : ZeroEntity, ISupportsTrees, new() 
  where TStore : ITreeEntityStore<TModel>
{
  public ZeroApiTreeEntityStoreController(TStore store) : base(store) { }


  protected async Task<ActionResult<Paged>> GetChildModels<T>(string parentId, ListQuery<TModel> query) where T : BasicModel<TModel>
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    Paged<TModel> result = await Store.LoadChildren(parentId, query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<TModel, T>(result);
  }


  protected async Task<ActionResult<Paged>> GetChildModels<T, TIndex>(string parentId, ListQuery<TModel> query) where T : BasicModel<TModel> where TIndex : AbstractCommonApiForIndexes, new()
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    Paged<TModel> result = await Store.LoadChildren<TIndex>(parentId, query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<TModel, T>(result);
  }


  protected async Task<ActionResult<Result>> SortModels(string[] ids)
  {
    return await Store.Sort(ids);
  }


  protected async Task<ActionResult<Result>> MoveModel<TEdit>(string pageId, string newParentId) where TEdit : DisplayModel<TModel>
  {
    return await PutOperation<TEdit>(async () => await Store.Move(pageId, newParentId));
  }


  protected async Task<ActionResult<Result>> CopyModel<TEdit>(string pageId, string newParentId) where TEdit : DisplayModel<TModel>
  {
    return await PutOperation<TEdit>(async () => await Store.Copy(pageId, newParentId));
  }


  protected async Task<ActionResult<Result>> CopyModelWithDescendants<TEdit>(string pageId, string newParentId) where TEdit : DisplayModel<TModel>
  {
    return await PutOperation<TEdit>(async () => await Store.CopyWithDescendants(pageId, newParentId));
  }


  protected async Task<ActionResult<Result>> DeleteModelWithDescendants(string id)
  {
    TModel model = await Store.Load(id);

    if (model == null)
    {
      return NotFound();
    }

    Result<string[]> result = await Store.DeleteWithDescendants(model);

    return result.WithoutModel();
  }


  async Task<ActionResult<Result>> PutOperation<TEdit>(Func<Task<Result<TModel>>> action) where TEdit : DisplayModel<TModel>
  {
    Result<TModel> result = await action();

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return Mapper.Map<TModel, TEdit>(result);
  }
}
