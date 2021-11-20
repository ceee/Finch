using Microsoft.AspNetCore.Http;

namespace zero.Applications;

public interface IApplicationResolverHandler : IHandler
{
  Application Resolve(HttpRequest request, IEnumerable<Application> applications);
}