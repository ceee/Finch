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


    public async Task<Result<string>> Add([FromBody] ZeroEntity model)
    {
      Result<Preview> preview = await Api.Add(model);
      return Result<string>.From(preview, preview.Model?.Id);
    }


    public async Task<Result<string>> Update([FromQuery] string id, [FromBody] ZeroEntity model)
    {
      Result<Preview> preview = await Api.Update(id, model);
      return Result<string>.From(preview, id);
    }
  }
}
