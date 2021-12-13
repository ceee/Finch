using Microsoft.AspNetCore.Mvc;

namespace zero.ApiTry
{
  public class FrontendController : Controller
  {
    public ActionResult Index()
    {
      return new ContentResult()
      {
        Content = "This is our <b>frontend</b><br>url:" + Request.Path,
        ContentType = "text/html",
        StatusCode = 200
      };
    }
  }
}