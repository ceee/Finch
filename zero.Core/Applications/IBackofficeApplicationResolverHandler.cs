using Microsoft.AspNetCore.Http;

namespace zero.Applications;

public interface IBackofficeApplicationResolverHandler : IHandler
{
  bool TryResolve(HttpContext context, IEnumerable<Application> applications, ZeroUser user, out Application resolved);
}