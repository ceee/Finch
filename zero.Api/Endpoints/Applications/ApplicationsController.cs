using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Applications;

[ZeroSystemApi]
public class ApplicationsController : ZeroApiEntityStoreController<Application, IApplicationStore>
{
  public ApplicationsController(IApplicationStore store) : base(store) { }


  [HttpGet("empty")]
  [ZeroAuthorize(ApplicationPermissions.Create)]
  public virtual Task<ActionResult<ApplicationEdit>> Empty(string flavor = null) => EmptyModel<ApplicationEdit>(flavor);


  [HttpGet("{id}")]
  [ZeroAuthorize(ApplicationPermissions.Read)]
  public virtual Task<ActionResult<ApplicationEdit>> Get(string id, string changeVector = null) => GetModel<ApplicationEdit>(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(ApplicationPermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Application> query) => GetModels<ApplicationBasic>(query);


  [HttpPost("")]
  [ZeroAuthorize(ApplicationPermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(ApplicationSave saveModel) => CreateModel<ApplicationSave, ApplicationEdit>(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(ApplicationPermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, ApplicationSave updateModel, [FromQuery] string changeToken = null) => UpdateModel<ApplicationSave, ApplicationEdit>(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(ApplicationPermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}