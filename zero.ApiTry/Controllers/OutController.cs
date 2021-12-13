using Microsoft.AspNetCore.Mvc;

namespace zero.ApiTry
{
  [ApiController]
  public class OutController : ControllerBase
  {
    [HttpGet("get")]
    public ActionResult Get()
    {
      return new JsonResult(new
      {
        success = true,
        text = "Out",
        url = Url.Action()
      });
    }
  }
}