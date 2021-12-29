using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Indexes;

namespace zero.Api.Abstractions;

public abstract class ZeroApiEntityStoreController<TModel, TStore> : ZeroApiController 
  where TModel : ZeroEntity, new() 
  where TStore : IEntityStore<TModel>
{
  protected TStore Store { get; set; }

  protected ZeroApiEntityStoreOperations<TModel, TStore> Ops { get; set; }

  public ZeroApiEntityStoreController(TStore store)
  {
    Store = store;
    Ops = new ZeroApiEntityStoreOperations<TModel, TStore>(this, store);
  }


  protected Task<ActionResult<T>> EmptyModel<T>(string flavorAlias = null, Action<T> modify = null) => Ops.EmptyModel<T>(flavorAlias, modify);
  protected Task<ActionResult<TModel>> EmptyModel(string flavorAlias = null, Action<TModel> modify = null) => Ops.EmptyModel(flavorAlias, modify);
  protected Task<ActionResult<T>> GetModel<T>(string id, string changeVector = null) => Ops.GetModel<T>(id, changeVector);
  protected Task<ActionResult<TModel>> GetModel(string id, string changeVector = null) => Ops.GetModel(id, changeVector);
  protected Task<ActionResult<Paged>> GetModels<T>(ListQuery<TModel> query) => Ops.GetModels<T>(query);
  protected Task<ActionResult<Paged>> GetModels(ListQuery<TModel> query) => Ops.GetModels(query);
  protected Task<ActionResult<Paged>> GetModelsByIndex<T, TIndex>(ListQuery<TModel> query) where TIndex : AbstractCommonApiForIndexes, new() => Ops.GetModelsByIndex<T, TIndex>(query);
  protected Task<ActionResult<Paged>> GetModelsByIndex<TIndex>(ListQuery<TModel> query) where TIndex : AbstractCommonApiForIndexes, new() => Ops.GetModelsByIndex<TIndex>(query);
  protected Task<ActionResult<Result>> CreateModel<T, TEdit>(T saveModel) where T : ISupportsFlavors => Ops.CreateModel<T, TEdit>(saveModel);
  protected Task<ActionResult<Result>> CreateModel(TModel saveModel) => Ops.CreateModel(saveModel);
  protected Task<ActionResult<Result>> UpdateModel<T, TEdit>(string id, T updateModel, string changeToken = null) where T : ZeroIdEntity => Ops.UpdateModel<T, TEdit>(id, updateModel, changeToken);
  protected Task<ActionResult<Result>> UpdateModel(string id, TModel updateModel, string changeToken = null) => Ops.UpdateModel(id, updateModel, changeToken);
  protected Task<ActionResult<Result>> SortModels(string[] ids) => Ops.SortModels(ids);
  protected Task<ActionResult<Result>> DeleteModel(string id) => Ops.DeleteModel(id);
}
