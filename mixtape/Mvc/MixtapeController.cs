using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.Mvc;

public abstract class MixtapeController : Controller
{
  IMixtapeContext _context;
  public IMixtapeContext Context => _context ??= HttpContext?.RequestServices?.GetService<IMixtapeContext>();
}