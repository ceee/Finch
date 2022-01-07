using Microsoft.AspNetCore.Http;

namespace zero.Api;

public class ApiApplicationResolverHandler : IBackofficeApplicationResolverHandler
{
  readonly IZeroOptions options;

  public ApiApplicationResolverHandler(IZeroOptions options)
  {
    this.options = options;
  }


  public bool TryResolve(HttpContext context, IEnumerable<Application> applications, ZeroUser user, out Application resolved)
  {
    string path = options.ZeroPath.EnsureStartsWith('/').TrimEnd('/');
    
    if (!context.Request.Path.ToString().StartsWith(path))
    {
      resolved = null;
      return false;
    }

    string appKey = context.Request.Path.Value.Substring(path.Length).TrimStart('/').Split('/').ElementAtOrDefault(1);

    if (appKey.HasValue())
    {
      resolved = applications.FirstOrDefault(x => x.Alias == appKey);
      return resolved != null;
    }

    resolved = null;
    return false;
  }
}