using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Integrations;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  //[ZeroAuthorize(Permissions.Sections.PREFIX + "commerce", PermissionsValue.Read)]
  public class IntegrationsController : BackofficeController
  {
    IIntegrationsCollection Collection;

    public IntegrationsController(IIntegrationsCollection collection)
    {
      Collection = collection;
      Collection.WithInactive();
    }


    public EditModel<IIntegration> GetEmpty([FromQuery] string alias) => Edit(Collection.GetEmpty(alias));


    public async Task<EditModel<IIntegration>> GetByAlias([FromQuery] string alias) => Edit(await Collection.GetByAlias(alias));


    public async Task<ListResult<IIntegration>> GetByQuery([FromQuery] ListQuery<IIntegration> query) => await Collection.GetByQuery(query);


    public async Task<IList<IntegrationTypeWithStatus>> GetTypes() => await Collection.GetTypesWithStatus();


    [HttpPost]
    public async Task<EntityResult<IIntegration>> Save([FromBody] IIntegration model) => await Collection.Save(model);

    [HttpPost]
    public async Task<EntityResult<IIntegration>> SaveActiveState([FromBody] Integration model) => model.IsActive ? await Collection.Activate(model.Alias) : await Collection.Deactivate(model.Alias);

    [HttpDelete]
    public async Task<EntityResult<IIntegration>> Delete([FromQuery] string alias) => await Collection.Delete(alias);
  }
}