using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class PageTreeController : BackofficeController
  {
    IPageTreeApi Api;

    public PageTreeController(IPageTreeApi api)
    {
      Api = api;
    }


    public async Task<IList<TreeItem>> GetChildren([FromQuery] string parent = null, [FromQuery] string active = null, [FromQuery] string search = null)
    {
      return await Api.GetChildren(parent, active, search);
    }
  }
}
