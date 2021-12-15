using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Mails;

public class MailTemplatesController : ZeroApiEntityStoreController<MailTemplate, IMailTemplatesStore>
{
  public MailTemplatesController(IMailTemplatesStore store) : base(store)
  {

  }


  [HttpGet("empty")]
  [ZeroAuthorize(MailPermissions.Create)]
  public virtual Task<ActionResult<MailEdit>> Empty(string flavor = null) => EmptyModel<MailEdit>(flavor);


  [HttpGet("{id}")]
  [ZeroAuthorize(MailPermissions.Read)]
  public virtual Task<ActionResult<MailEdit>> Get(string id, string changeVector = null) => GetModel<MailEdit>(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(MailPermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<MailTemplate> query)
  {
    query.SearchFor(entity => entity.Name, entity => entity.Key, entity => entity.Subject);
    return GetModelsByIndex<MailBasic, zero_Api_MailTemplates_Listing>(query);
  }


  [HttpPost("")]
  [ZeroAuthorize(MailPermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(MailSave saveModel) => CreateModel<MailSave, MailEdit>(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(MailPermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, MailSave updateModel, [FromQuery] string changeToken = null) => UpdateModel<MailSave, MailEdit>(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(MailPermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}