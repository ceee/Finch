using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Languages;

public class LanguagesController : ZeroApiEntityStoreController<Language, ILanguageStore>
{
  public LanguagesController(ILanguageStore store) : base(store)
  {

  }


  [HttpGet("empty")]
  [ZeroAuthorize(LanguagePermissions.Create)]
  public virtual Task<ActionResult<LanguageEdit>> Empty() => EmptyModel<LanguageEdit>();


  [HttpGet("{id}")]
  [ZeroAuthorize(LanguagePermissions.Read)]
  public virtual Task<ActionResult<LanguageEdit>> Get(string id, string changeVector = null) => GetModel<LanguageEdit>(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(LanguagePermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Language> query) => GetModels<LanguageBasic, zero_Api_Languages_Listing>(query);


  [HttpPost("")]
  [ZeroAuthorize(LanguagePermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(LanguageSave saveModel) => CreateModel<LanguageSave, LanguageEdit>(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(LanguagePermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, LanguageSave updateModel) => UpdateModel<LanguageSave, LanguageEdit>(id, updateModel);


  [HttpDelete("{id}")]
  [ZeroAuthorize(LanguagePermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}