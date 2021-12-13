using Microsoft.AspNetCore.Mvc;

namespace zero.ApiTry
{
  [ZeroSystemApi]
  public class ByeController : AppApiController
  {
    [HttpGet("")]
    public ActionResult Get()
    {
      return new JsonResult(new
      {
        success = true,
        text = "Bye",
        url = Url.Action()
      });
    }
  }
}
