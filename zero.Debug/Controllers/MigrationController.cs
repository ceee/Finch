using Microsoft.AspNetCore.Mvc;
using zero.Core.Database;

namespace zero.Debug.Controllers
{
  public partial class MigrationController : Controller
  {
    private IZeroStore Store { get; set; }

    public MigrationController(IZeroStore store)
    {
      Store = store;
    }
  }
}