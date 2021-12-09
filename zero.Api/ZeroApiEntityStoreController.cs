using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Indexes;

namespace zero.Api.Controllers;

public abstract class ZeroApiEntityStoreController<TModel, TStore> : ZeroApiController 
  where TModel : ZeroEntity, new() 
  where TStore : IEntityStore<TModel>
{
  protected TStore Store { get; set; }

  public ZeroApiEntityStoreController(TStore store)
  {
    Store = store;
  }


  protected string GetActionMethod { get; set; } = "Get";

  protected virtual string GetAction(TModel model)
  {
    return Url.Action(GetActionMethod, new { id = model.Id });
  }


  protected async Task<ActionResult<T>> EmptyModel<T>(string flavorAlias = null, Action<T> modify = null) where T : DisplayModel<TModel>
  {
    TModel model = await Store.Empty(flavorAlias);

    if (model == null)
    {
      return NotFound();
    }

    T result = Mapper.Map<TModel, T>(model);
    modify?.Invoke(result);
    return result;
  }


  protected async Task<ActionResult<T>> GetModel<T>(string id, string changeVector = null) where T : DisplayModel<TModel>
  {
    TModel model = await Store.Load(id, changeVector);

    if (model == null)
    {
      return NotFound();
    }

    HttpContext.Items[ApiConstants.ChangeVector] = Store.GetChangeVector(model);

    return Mapper.Map<TModel, T>(model);
  }


  protected async Task<ActionResult<Paged>> GetModels<T>(ListQuery<TModel> query) where T : BasicModel<TModel>
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    query.SearchSelector ??= x => x.Name;
    Paged<TModel> result = await Store.Load(query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<TModel, T>(result, (src, dest) =>
    {
      dest.Link = GetAction(src);
    });
  }


  protected async Task<ActionResult<Paged>> GetModels<T, TIndex>(ListQuery<TModel> query) where T : BasicModel<TModel> where TIndex : AbstractCommonApiForIndexes, new()
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    query.SearchSelector ??= x => x.Name;
    Paged<TModel> result = await Store.Load<TIndex>(query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<TModel, T>(result, (src, dest) =>
    {
      dest.Link = GetAction(src);
    });
  }


  protected async Task<ActionResult<Result>> CreateModel<T, TEdit>(T saveModel) where T : SaveModel<TModel> where TEdit : DisplayModel<TModel>
  {
    TModel emptyModel = saveModel.Flavor.IsNullOrEmpty() ? await Store.Empty() : await Store.Empty(saveModel.Flavor);
    TModel model = Mapper.Map(saveModel, emptyModel);
    Result<TModel> result = await Store.Create(model);

    bool minimalResponse = Hints.ResponsePreference == ApiResponsePreference.Minimal;

    if (result.IsSuccess)
    {
      Result<TEdit> mappedResult = Mapper.Map<TModel, TEdit>(result);
      return Created(GetAction(model), minimalResponse ? null : mappedResult);
    }

    if (minimalResponse)
    {
      return result.WithoutModel();
    }

    return Mapper.Map<TModel, TEdit>(result);
  }


  protected async Task<ActionResult<Result>> UpdateModel<T, TEdit>(string id, T updateModel, string changeToken = null) where T : SaveModel<TModel> where TEdit : DisplayModel<TModel>
  {
    if (id != updateModel.Id)
    {
      return BadRequest(ApiConstants.HttpErrors.NoIdMatchOnUpdate);
    }

    TModel model = await Store.Load(id);

    if (model == null)
    {
      return BadRequest(ApiConstants.HttpErrors.IdNotFound);
    }

    if (!changeToken.IsNullOrEmpty() && Store.GetChangeVector(model) != changeToken)
    {
      return BadRequest(ApiConstants.HttpErrors.ChangeTokenMismatch);
    }

    Mapper.Map(updateModel, model);

    Result<TModel> result = await Store.Update(model);

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    // TODO add Preference-Applied header, see https://github.com/microsoft/api-guidelines/blob/vNext/Guidelines.md#76-standard-response-headers
    return Mapper.Map<TModel, TEdit>(result);
  }


  protected async Task<ActionResult<Result>> DeleteModel(string id)
  {
    TModel model = await Store.Load(id);

    if (model == null)
    {
      return NotFound();
    }

    Result<TModel> result = await Store.Delete(model);

    return result.WithoutModel();
  }
}
