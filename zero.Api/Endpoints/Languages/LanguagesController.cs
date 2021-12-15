using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Languages;

public class LanguagesController : ZeroApiEntityStoreController<Language, ILanguageStore>
{
  public LanguagesController(ILanguageStore store) : base(store)
  {

  }


  [HttpGet("empty")]
  [ZeroAuthorize(LanguagePermissions.Create)]
  public virtual Task<ActionResult<Language>> Empty(string flavor = null) => EmptyModel(flavor);


  [HttpGet("{id}")]
  [ZeroAuthorize(LanguagePermissions.Read)]
  public virtual Task<ActionResult<Language>> Get(string id, string changeVector = null) => GetModel(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(LanguagePermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Language> query) => GetModelsByIndex<LanguageBasic, zero_Api_Languages_Listing>(query);


  [HttpPost("")]
  [ZeroAuthorize(LanguagePermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(Language saveModel) => CreateModel(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(LanguagePermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, Language updateModel, [FromQuery] string changeToken = null) => UpdateModel(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(LanguagePermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}