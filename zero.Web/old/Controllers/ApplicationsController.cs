using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Read)]
  public class ApplicationsController : BackofficeController
  {
    IApplicationsApi Api;

    public ApplicationsController(IApplicationsApi api)
    {
      Api = api;
      IsCoreDatabase = true;
    }


    public EditModel<Application> GetEmpty([FromServices] Application blueprint) => Edit(blueprint);


    public async Task<EditModel<Application>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<IList<Application>> GetAll() => await Api.GetAll();


    public async Task<ListResult<Application>> GetByQuery([FromQuery] ListBackofficeQuery<Application> query) => await Api.GetByQuery(query);


    public IReadOnlyCollection<Feature> GetAllFeatures() => Options.Features.GetAllItems();


    [HttpPost]
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Update)]
    public async Task<EntityResult<Application>> Save([FromBody] Application model) => await Api.Save(model);


    [HttpDelete]
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Update)]
    public async Task<EntityResult<Application>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
