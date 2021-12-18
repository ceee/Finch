using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Indexes;

namespace zero.Api.Abstractions;

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
    return "/"; // TODO Url.Action does not work anymore,
                // probably due to ZeroApiControllerModelConvention, which rewrites AttributeRouteModel
    //return Url.Action(GetActionMethod, ControllerContext.ActionDescriptor.ControllerName, new { id = model.Id, changeVector = default(string) });
  }


  protected async Task<ActionResult<T>> EmptyModel<T>(string flavorAlias = null, Action<T> modify = null)
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


  protected async Task<ActionResult<TModel>> EmptyModel(string flavorAlias = null, Action<TModel> modify = null)
  {
    TModel model = await Store.Empty(flavorAlias);

    if (model == null)
    {
      return NotFound();
    }

    modify?.Invoke(model);
    return model;
  }


  protected async Task<ActionResult<T>> GetModel<T>(string id, string changeVector = null)
  {
    TModel model = await Store.Load(id, changeVector);

    if (model == null)
    {
      return NotFound();
    }

    HttpContext.Items[ApiConstants.ChangeToken] = Store.GetChangeToken(model);

    return Mapper.Map<TModel, T>(model);
  }


  protected async Task<ActionResult<TModel>> GetModel(string id, string changeVector = null)
  {
    TModel model = await Store.Load(id, changeVector);

    if (model == null)
    {
      return NotFound();
    }

    HttpContext.Items[ApiConstants.ChangeToken] = Store.GetChangeToken(model);

    return model;
  }


  protected async Task<ActionResult<Paged>> GetModels<T>(ListQuery<TModel> query)
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    query.SearchSelector ??= x => x.Name;
    Paged<TModel> result = await Store.Load(query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<TModel, T>(result);
  }


  protected async Task<ActionResult<Paged>> GetModels(ListQuery<TModel> query)
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    query.SearchSelector ??= x => x.Name;
    Paged<TModel> result = await Store.Load(query.Page, query.PageSize, q => q.Filter(query));
    return result;
  }


  protected async Task<ActionResult<Paged>> GetModelsByIndex<T, TIndex>(ListQuery<TModel> query) where TIndex : AbstractCommonApiForIndexes, new()
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    query.SearchSelector ??= x => x.Name;
    Paged<TModel> result = await Store.Load<TIndex>(query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<TModel, T>(result);
  }


  protected async Task<ActionResult<Paged>> GetModelsByIndex<TIndex>(ListQuery<TModel> query) where TIndex : AbstractCommonApiForIndexes, new()
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    query.SearchSelector ??= x => x.Name;
    Paged<TModel> result = await Store.Load<TIndex>(query.Page, query.PageSize, q => q.Filter(query));
    return result;
  }


  protected async Task<ActionResult<Result>> CreateModel<T, TEdit>(T saveModel) where T : ISupportsFlavors
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

    return result.WithoutModel();
  }


  protected async Task<ActionResult<Result>> CreateModel(TModel saveModel)
  {
    Result<TModel> result = await Store.Create(saveModel);

    bool minimalResponse = Hints.ResponsePreference == ApiResponsePreference.Minimal;

    if (result.IsSuccess)
    {
      return Created(GetAction(saveModel), minimalResponse ? null : saveModel);
    }

    return result.WithoutModel();
  }


  protected async Task<ActionResult<Result>> UpdateModel<T, TEdit>(string id, T updateModel, string changeToken = null) where T : ZeroIdEntity
  {
    if (id != updateModel.Id)
    {
      return BadRequest(Result.Fail(nameof(id), "@errors.onupdate.noidmatch"));
    }

    TModel model = await Store.Load(id);

    if (model == null)
    {
      return BadRequest(Result.Fail(nameof(id), "@errors.idnotfound"));
    }

    string storedChangeToken = Store.GetChangeToken(model);

    if (!changeToken.IsNullOrEmpty() && storedChangeToken != changeToken)
    {
      return BadRequest(Result.Fail("@errors.onupdate.changetokenmismatch"));
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


  protected async Task<ActionResult<Result>> UpdateModel(string id, TModel updateModel, string changeToken = null)
  {
    if (id != updateModel.Id)
    {
      return BadRequest(Result.Fail(nameof(id), "@errors.onupdate.noidmatch"));
    }

    // TODO throws error on save: Attempted to associate a different object with id ...
    // we need to map props to new object
    //TModel model = await Store.Load(id);

    //if (model == null)
    //{
    //  return BadRequest(Result.Fail(nameof(id), "@errors.idnotfound"));
    //}

    //string storedChangeToken = Store.GetChangeToken(model);

    //if (!changeToken.IsNullOrEmpty() && storedChangeToken != changeToken)
    //{
    //  return BadRequest(Result.Fail("@errors.onupdate.changetokenmismatch"));
    //}

    Result<TModel> result = await Store.Update(updateModel);

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return result;
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
