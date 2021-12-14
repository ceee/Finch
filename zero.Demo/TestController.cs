using Microsoft.AspNetCore.Mvc;
using zero.Context;
using zero.Identity;
using zero.Stores;

namespace zero.Demo.Controllers;

public class TestController : ControllerBase
{
  [HttpGet("")]
  public ActionResult Get(string id, string changeVector = null)
  {
    return new JsonResult(new
    {
      id = id
    });
  }


  [HttpGet("/test/url")]
  public ActionResult GetUrl()
  {
    return new JsonResult(new
    {
      url = Url.Action("Get", new { id = "myid" })
    });
  }

  //[HttpGet]
  //public async Task<ActionResult> Scoping([FromServices] IZeroContext ctx, [FromServices] IStoreOperations ops)
  //{
  //  string scopeA = null;
  //  string scopeB = null;
  //  string scopeC = null;
  //  List<ZeroUser> usersA = new();
  //  List<ZeroUser> usersB = new();

  //  scopeA = ctx.Store.ResolvedDatabase;

  //  using (var scope = ctx.CreateScope("laola"))
  //  {
  //    scopeB = ctx.Store.ResolvedDatabase;
  //    usersA = await ops.LoadAll<ZeroUser>();
  //  }

  //  scopeC = ctx.Store.ResolvedDatabase;
  //  usersB = await ops.LoadAll<ZeroUser>();



  //  return Json(new
  //  {
  //    scopeA,
  //    scopeB,
  //    scopeC,
  //    usersA,
  //    usersB
  //  });
  //}
}