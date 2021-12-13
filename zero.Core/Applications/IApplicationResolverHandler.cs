using Microsoft.AspNetCore.Http;

namespace zero.Applications;

public interface IApplicationResolverHandler : IHandler
{
  bool TryResolve(HttpContext context, IEnumerable<Application> applications, out Application resolved);
}