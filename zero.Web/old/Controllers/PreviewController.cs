using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  public class PreviewController : BackofficeController
  {
    IPreviewApi Api;

    public PreviewController(IPreviewApi api)
    {
      Api = api;
    }


    public async Task<EditModel<Preview>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<EntityResult<string>> Add([FromBody] ZeroEntity model)
    {
      EntityResult<Preview> preview = await Api.Add(model);
      return EntityResult<string>.From(preview, preview.Model?.Id);
    }


    public async Task<EntityResult<string>> Update([FromQuery] string id, [FromBody] ZeroEntity model)
    {
      EntityResult<Preview> preview = await Api.Update(id, model);
      return EntityResult<string>.From(preview, id);
    }
  }
}
