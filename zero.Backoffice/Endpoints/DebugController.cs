using Microsoft.AspNetCore.Mvc;
using zero.Api.Filters;
using zero.Backoffice.Services;
using zero.Identity.Models;

namespace zero.Backoffice;

[ZeroSystemApi]
[ZeroAuthorize(false)]
public class DebugController : ZeroBackofficeController
{
  [HttpGet("icons")]
  public async Task<ActionResult> GetIcons([FromServices] IIconRepository icons)
  {
    return Ok(await icons.GetSets());
  }
}