using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace zero.Api;

public class ApiApplicationResolverHandler : IBackofficeApplicationResolverHandler
{
  public bool TryResolve(HttpContext context, IEnumerable<Application> applications, ZeroUser user, out Application resolved)
  {
    var routeValues = context.Features.Get<IRouteValuesFeature>().RouteValues;

    if (routeValues.ContainsKey("zero_app_key"))
    {
      string appKey = routeValues["zero_app_key"] as string;
      resolved = applications.FirstOrDefault(x => x.Alias == appKey);
      return resolved != null;
    }

    resolved = null;
    return false;
  }
}