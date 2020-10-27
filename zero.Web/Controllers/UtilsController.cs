using Microsoft.AspNetCore.Mvc;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  public class UtilsController : BackofficeController
  {
    public string GenerateAlias([FromQuery] string name) => Safenames.Alias(name);
  }
}
