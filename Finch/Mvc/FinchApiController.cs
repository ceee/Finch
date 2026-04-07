using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Mvc;

[ApiController]
public abstract class FinchApiController : ControllerBase
{
  IFinchContext _context;
  public IFinchContext Context => _context ?? (_context = HttpContext?.RequestServices?.GetService<IFinchContext>());
}