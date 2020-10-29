using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  public class PagesController : BackofficeController
  {
    IPagesApi Api;
    IRevisionsApi RevisionsApi;

    public PagesController(IPagesApi api, IRevisionsApi revisionsApi)
    {
      Api = api;
      RevisionsApi = revisionsApi;
    }


    public async Task<IList<PageType>> GetAllowedPageTypes([FromQuery] string parent = null) => await Api.GetAllowedPageTypes(parent);

 
    public PageType GetPageType([FromQuery] string alias) => Api.GetPageType(alias);

    public async Task<PageEditModel<IPage>> GetById([FromQuery] string id)
    {
      IPage entity = await Api.GetById(id);

      if (entity == null)
      {
        return null;
      }

      return Edit<IPage, PageEditModel<IPage>>(new PageEditModel<IPage>()
      {
        Entity = entity,
        //Revisions = await RevisionsApi.GetPaged<IPage>(id),
        PageType = Api.GetPageType(entity.PageTypeAlias)
      });
    }


    public EditModel<IPage> GetEmpty([FromQuery] string type, [FromQuery] string parent = null)
    {
      PageType pageType = Api.GetPageType(type);
      IPage model = Activator.CreateInstance(pageType.ContentType) as IPage;

      model.PageTypeAlias = type;
      model.ParentId = parent;

      return Edit(model);
    }


    public async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids)
    {
      IReadOnlyCollection<PageType> pageTypes = Options.Pages.GetAllItems();

      return Previews(await Api.GetByIds(ids.ToArray()), item =>
      {
        PageType pageType = pageTypes.FirstOrDefault(x => x.Alias == item.PageTypeAlias);

        return new PreviewModel()
        {
          Id = item.Id,
          Icon = pageType?.Icon ?? "fth-folder",
          Name = item.Name,
          //Text = item.
        };
      });
    }


    public async Task<ListResult<Revision>> GetRevisions([FromQuery] string id, [FromQuery] int page = 1) => await RevisionsApi.GetPaged<IPage>(id, page);


    public async Task<EntityResult<IPage>> Save([FromBody] IPage model) => await Api.Save(model);


    [HttpPost]
    public async Task<EntityResult<IList<IPage>>> SaveSorting([FromBody] string[] ids) => await Api.SaveSorting(ids);


    [HttpPost]
    public async Task<EntityResult<IPage>> Move([FromBody] ActionCopyModel model) => await Api.Move(model.Id, model.DestinationId);


    [HttpPost]
    public async Task<EntityResult<IPage>> Copy([FromBody] ActionCopyModel model) => await Api.Copy(model.Id, model.DestinationId, model.IncludeDescendants);


    [HttpPost]
    public async Task<EntityResult<string[]>> Restore([FromBody] ActionCopyModel model) => await Api.Restore(model.Id, model.IncludeDescendants);


    public async Task<EntityResult<string[]>> Delete([FromQuery] string id) => await Api.Delete(id, true);
  }
}
