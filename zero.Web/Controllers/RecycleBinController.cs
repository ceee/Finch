using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  public class RecycleBinController : BackofficeController
  {
    IRecycleBinApi Api;

    public RecycleBinController(IRecycleBinApi api)
    {
      Api = api;
    }

    public async Task<IActionResult> GetByQuery([FromQuery] RecycleBinListQuery query) => Json(await Api.GetByQuery(query));

    public async Task<IActionResult> GetCountByOperation([FromQuery] string operationId) => Json(await Api.GetCountByOperation(operationId));
  }
}
