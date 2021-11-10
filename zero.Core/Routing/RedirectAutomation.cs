using Raven.Client.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database.Indexes;

namespace zero.Core.Routing
{
  public class RedirectAutomation : IRedirectAutomation
  {
    protected IRouteRedirectCollection Collection { get; set; }


    public RedirectAutomation(IRouteRedirectCollection collection)
    {
      Collection = collection;
    }


    /// <inheritdoc />
    public async Task<RouteRedirect> AddForRoute(string oldUrl, string newUrl, RouteRedirectType redirectType = RouteRedirectType.Permanent)
    {
      List<RouteRedirect> redirects = await Collection.Session.Query<RouteRedirect, RouteRedirects_ByUrl>().ProjectInto<RouteRedirect>().Where(x => x.SourceUrl == oldUrl || x.TargetUrl == oldUrl).ToListAsync();

      RouteRedirect redirect = redirects.FirstOrDefault(x => x.SourceUrl == oldUrl) ?? new();
      redirect.IsActive = true;
      redirect.IsAutomated = true;
      redirect.RedirectType = redirectType;
      redirect.SourceUrl = oldUrl;
      redirect.TargetUrl = newUrl;

      await Collection.Save(redirect);

      // TODO this does not work as expected yet
      //foreach (RouteRedirect oldTargetRedirect in redirects)
      //{
      //  if (oldTargetRedirect.TargetUrl == oldUrl)
      //  {
      //    oldTargetRedirect.IsActive = true;
      //    oldTargetRedirect.IsAutomated = true;
      //    oldTargetRedirect.RedirectType = redirectType;
      //    oldTargetRedirect.TargetUrl = newUrl;

      //    if (oldTargetRedirect.TargetUrl != oldTargetRedirect.SourceUrl)
      //    {
      //      await Collection.Save(oldTargetRedirect);
      //    }
      //    else
      //    {
      //      await Collection.Delete(oldTargetRedirect);
      //    }
      //  }
      //}

      return redirect;
    }


    /// <inheritdoc />
    public async Task DeleteForRoutes(params Route[] routes)
    {
      if (routes.Length < 1)
      {
        return;
      }

      await Collection.Purge($"where c.{nameof(RouteRedirect.IsAutomated)} = $automated and c.{nameof(RouteRedirect.TargetUrl)} IN ($urls)", new Raven.Client.Parameters()
      {
        { "automated", true },
        { "url", routes.Select(x => x.Url).ToArray() }
      });
    }
  }



  public interface IRedirectAutomation
  {
    /// <summary>
    /// Add or update all affected redirects for a route
    /// </summary>
    Task<RouteRedirect> AddForRoute(string sourceUrl, string targetUrl, RouteRedirectType redirectType = RouteRedirectType.Permanent);

    /// <summary>
    /// Delete all redirects which have the passed route URLs as their target.
    /// This is only done for automated redirects and not for redirects which are custom-created.
    /// </summary>
    Task DeleteForRoutes(params Route[] routes);
  }
}
