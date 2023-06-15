using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Mvc;

public abstract class ZeroController : Controller
{
  IZeroContext _context;
  public IZeroContext Context => _context ?? (_context = HttpContext?.RequestServices?.GetService<IZeroContext>());
}