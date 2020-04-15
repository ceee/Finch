using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class ApplicationsController : BackofficeController
  {
    private IApplicationsApi Api { get; set; }

    public ApplicationsController(IZeroConfiguration config, IApplicationsApi api) : base(config)
    {
      Api = api;
    }


    public async Task<IActionResult> GetAll()
    {
      return Json(await Api.GetAll());
    }
  }
}
