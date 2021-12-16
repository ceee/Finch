using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Indexes;

namespace zero.Api.Abstractions;

public abstract class ZeroApiTreeEntityStoreController<TModel, TStore> : ZeroApiEntityStoreController<TModel, TStore> 
  where TModel : ZeroEntity, ISupportsTrees, new() 
  where TStore : ITreeEntityStore<TModel>
{
  public ZeroApiTreeEntityStoreController(TStore store) : base(store) { }


  protected async Task<ActionResult<Paged>> GetChildModels<T>(string parentId, ListQuery<TModel> query)
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    Paged<TModel> result = await Store.LoadChildren(parentId, query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<TModel, T>(result);
  }


  protected async Task<ActionResult<Paged>> GetChildModels(string parentId, ListQuery<TModel> query)
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    Paged<TModel> result = await Store.LoadChildren(parentId, query.Page, query.PageSize, q => q.Filter(query));

    return result;
  }


  protected async Task<ActionResult<Paged>> GetChildModelsByIndex<T, TIndex>(string parentId, ListQuery<TModel> query) where TIndex : AbstractCommonApiForIndexes, new()
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    Paged<TModel> result = await Store.LoadChildren<TIndex>(parentId, query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<TModel, T>(result);
  }


  protected async Task<ActionResult<Paged>> GetChildModelsByIndex<TIndex>(string parentId, ListQuery<TModel> query) where TIndex : AbstractCommonApiForIndexes, new()
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    Paged<TModel> result = await Store.LoadChildren<TIndex>(parentId, query.Page, query.PageSize, q => q.Filter(query));

    return result;
  }


  protected async Task<ActionResult<Result>> SortModels(string[] ids)
  {
    return await Store.Sort(ids);
  }


  protected async Task<ActionResult<Result>> MoveModel<TEdit>(string id, string newParentId)
  {
    return await PutOperation<TEdit>(async () => await Store.Move(id, newParentId));
  }


  protected async Task<ActionResult<Result>> MoveModel(string id, string newParentId)
  {
    return await PutOperation(async () => await Store.Move(id, newParentId));
  }


  protected async Task<ActionResult<Result>> CopyModel<TEdit>(string id, string newParentId)
  {
    return await PutOperation<TEdit>(async () => await Store.Copy(id, newParentId));
  }


  protected async Task<ActionResult<Result>> CopyModel(string id, string newParentId)
  {
    return await PutOperation(async () => await Store.Copy(id, newParentId));
  }


  protected async Task<ActionResult<Result>> CopyModelWithDescendants<TEdit>(string id, string newParentId)
  {
    return await PutOperation<TEdit>(async () => await Store.CopyWithDescendants(id, newParentId));
  }


  protected async Task<ActionResult<Result>> CopyModelWithDescendants(string id, string newParentId)
  {
    return await PutOperation(async () => await Store.CopyWithDescendants(id, newParentId));
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


  async Task<ActionResult<Result>> PutOperation<TEdit>(Func<Task<Result<TModel>>> action)
  {
    Result<TModel> result = await action();

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return Mapper.Map<TModel, TEdit>(result);
  }


  async Task<ActionResult<Result>> PutOperation(Func<Task<Result<TModel>>> action)
  {
    Result<TModel> result = await action();

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return result;
  }
}
