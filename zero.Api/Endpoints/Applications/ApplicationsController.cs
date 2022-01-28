using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Applications;

[ZeroSystemApi]
public class ApplicationsController : ZeroApiEntityStoreController<Application, IApplicationStore>
{
  public ApplicationsController(IApplicationStore store) : base(store) { }


  [HttpGet("empty")]
  [ZeroAuthorize(ApplicationPermissions.Create)]
  public virtual Task<ActionResult<Application>> Empty(string flavor = null) => EmptyModel(flavor);


  [HttpGet("{id}")]
  [ZeroAuthorize(ApplicationPermissions.Read)]
  public virtual Task<ActionResult<Application>> Get(string id, string changeVector = null) => GetModel(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(ApplicationPermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Application> query) => GetModels<ApplicationBasic>(query);


  [HttpPost("")]
  [ZeroAuthorize(ApplicationPermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(Application saveModel) => CreateModel(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(ApplicationPermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, Application updateModel, [FromQuery] string changeToken = null) => UpdateModel(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(ApplicationPermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}