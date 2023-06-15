using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Mvc;

[ApiController]
public abstract class ZeroApiController : ControllerBase
{
  IZeroContext _context;
  public IZeroContext Context => _context ?? (_context = HttpContext?.RequestServices?.GetService<IZeroContext>());
}