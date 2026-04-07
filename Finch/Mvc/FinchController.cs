using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Mvc;

public abstract class FinchController : Controller
{
  IFinchContext _context;
  public IFinchContext Context => _context ?? (_context = HttpContext?.RequestServices?.GetService<IFinchContext>());
}