//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using zero.Core.Collections;
//using zero.Core.Entities;
//using zero.Core.Integrations;
//using zero.Web.Models;

//namespace zero.Web.Controllers
//{
//  //[ZeroAuthorize(Permissions.Sections.PREFIX + "commerce", PermissionsValue.Read)]
//  public class IntegrationsController : BackofficeController
//  {
//    IIntegrationsCollection Collection;

//    public IntegrationsController(IIntegrationsCollection collection)
//    {
//      Collection = collection;
//    }


//    public EditModel<Integration> GetEmpty([FromQuery] string alias) => Edit(Collection.GetEmpty(alias));


//    public async Task<EditModel<Integration>> GetByAlias([FromQuery] string alias) => Edit(await Collection.GetByAlias(alias));


//    public async Task<Paged<Integration>> Load([FromQuery] ListQuery<Integration> query) => await Collection.Load(query);


//    public async Task<IList<IntegrationTypeWithStatus>> GetTypes() => await Collection.GetTypesWithStatus();


//    [HttpPost]
//    public async Task<Result<Integration>> Save([FromBody] Integration model) => await Collection.Save(model);

//    [HttpPost]
//    public async Task<Result<Integration>> SaveActiveState([FromBody] Integration model) => model.IsActive ? await Collection.Activate(model.Alias) : await Collection.Deactivate(model.Alias);

//    [HttpDelete]
//    public async Task<Result<Integration>> Delete([FromQuery] string alias) => await Collection.DeleteByAlias(alias);
//  }
//}