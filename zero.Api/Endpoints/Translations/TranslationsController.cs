using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Translations;

public class TranslationsController : ZeroApiEntityStoreController<Translation, ITranslationStore>
{
  public TranslationsController(ITranslationStore store) : base(store)
  {

  }


  [HttpGet("empty")]
  [ZeroAuthorize(TranslationPermissions.Create)]
  public virtual Task<ActionResult<TranslationEdit>> Empty() => EmptyModel<TranslationEdit>();


  [HttpGet("{id}")]
  [ZeroAuthorize(TranslationPermissions.Read)]
  public virtual Task<ActionResult<TranslationEdit>> Get(string id, string changeVector = null) => GetModel<TranslationEdit>(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(TranslationPermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Translation> query)
  {
    query.SearchFor(x => x.Key, x => x.Value);
    return GetModels<TranslationBasic, zero_Api_Translations_Listing>(query);
  }


  [HttpPost("")]
  [ZeroAuthorize(TranslationPermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(TranslationSave saveModel) => CreateModel<TranslationSave, TranslationEdit>(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(TranslationPermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, TranslationSave updateModel, [FromQuery] string changeToken = null) => UpdateModel<TranslationSave, TranslationEdit>(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(TranslationPermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}