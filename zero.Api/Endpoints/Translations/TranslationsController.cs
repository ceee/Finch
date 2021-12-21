using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Translations;

public class TranslationsController : ZeroApiEntityStoreController<Translation, ITranslationStore>
{
  public TranslationsController(ITranslationStore store) : base(store)
  {

  }


  [HttpGet("empty")]
  [ZeroAuthorize(TranslationPermissions.Create)]
  public virtual Task<ActionResult<Translation>> Empty(string flavor = null) => EmptyModel(flavor);


  [HttpGet("{id}")]
  [ZeroAuthorize(TranslationPermissions.Read)]
  public virtual Task<ActionResult<Translation>> Get(string id, string changeVector = null) => GetModel(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(TranslationPermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Translation> query)
  {
    query.SearchFor(x => x.Key, x => x.Value);
    return GetModelsByIndex<TranslationBasic, zero_Api_Translations_Listing>(query);
  }


  [HttpPost("")]
  [ZeroAuthorize(TranslationPermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(Translation saveModel) => CreateModel(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(TranslationPermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, Translation updateModel, [FromQuery] string changeToken = null) => UpdateModel(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(TranslationPermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}