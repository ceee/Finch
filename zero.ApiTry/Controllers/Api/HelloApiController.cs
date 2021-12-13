using Microsoft.AspNetCore.Mvc;

namespace zero.ApiTry
{
  public class HelloController : AppApiController
  {
    [HttpGet("")]
    public ActionResult Get()
    {
      return new JsonResult(new
      {
        success = true,
        text = "Hello"
      });
    }
  }
}
