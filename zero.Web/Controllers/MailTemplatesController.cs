using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core;
using zero.Core.Entities;
using zero.Web.Controllers;
using zero.Web.Models;

namespace zero.Commerce.Backoffice
{
  public class MailTemplatesController : BackofficeController
  {
    IMailTemplatesApi Api;

    public MailTemplatesController(IMailTemplatesApi api)
    {
      Api = api;
    }


    public EditModel<IMailTemplate> GetEmpty([FromServices] IMailTemplate blueprint) => Edit(blueprint);

    public async Task<EditModel<IMailTemplate>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));

    public async Task<ListResult<IMailTemplate>> GetAll([FromQuery] ListQuery<IMailTemplate> query) => await Api.GetByQuery(query);

    public async Task<IEnumerable<SelectModel>> GetForPicker() => (await Api.GetAll()).Select(x => new SelectModel()
    {
      Id = x.Id,
      Name = x.Name,
      IsActive = x.IsActive
    });


    public async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids) => Previews(await Api.GetByIds(ids.ToArray()), item => new PreviewModel()
    {
      Id = item.Id,
      Icon = "fth-mail",
      Name = item.Name
    });


    public async Task<EntityResult<IMailTemplate>> Save([FromBody] IMailTemplate model) => await Api.Save(model);


    public async Task<EntityResult<IMailTemplate>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
