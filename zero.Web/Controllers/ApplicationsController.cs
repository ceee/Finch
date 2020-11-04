using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
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
    }


    public EditModel<IApplication> GetEmpty([FromServices] IApplication blueprint) => Edit(blueprint);


    public async Task<EditModel<IApplication>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<IList<IApplication>> GetAll() => await Api.GetAll();


    public async Task<ListResult<IApplication>> GetByQuery([FromQuery] ListQuery<IApplication> query) => await Api.GetByQuery(query);


    public IReadOnlyCollection<IFeature> GetAllFeatures() => Options.Features.GetAllItems();


    [HttpPost]
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Update)]
    public async Task<EntityResult<IApplication>> Save([FromBody] IApplication model) => await Api.Save(model);


    [HttpDelete]
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Update)]
    public async Task<EntityResult<IApplication>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
