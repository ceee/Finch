using Raven.Client.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Routing
{
  public class PageRouteProvider : ZeroRouteProvider<Page>
  {
    public static string PAGE_TYPE_PARAM = "pageType";

    public static string PAGE_ID_PARAM = "pageId";

    protected IPageUrlBuilder UrlBuilder { get; set; }


    public PageRouteProvider(IPageUrlBuilder urlBuilder) : base(Constants.Pages.PageRouteProviderAlias)
    {
      UrlBuilder = urlBuilder;
    }


    /// <inheritdoc />
    public override Task<bool> IsRouteStale(RoutingContext context, Page previous, Page current)
    {
      bool compareUrlPart = !UrlBuilder.GetUrlPart(previous).Equals(UrlBuilder.GetUrlPart(current));
      return Task.FromResult(compareUrlPart || !previous.ParentId.Equals(current.ParentId));
    }


    /// <inheritdoc />
    public override async Task<Route> Create(RoutingContext context, Page model)
    {
      if (model is PageFolder)
      {
        return null;
      }

      IEnumerable<Page> parents = await GetParents(context, model);

      Route route = new();

      route.Id = Id(model);
      route.ReferenceId = model.Id;
      route.Url = UrlBuilder.GetUrl(model, parents);
      route.DependsOn(model.Id);
      route.DependsOn(parents.Select(x => x.Id).ToArray());
      route.Param(PAGE_TYPE_PARAM, model.PageTypeAlias);
      route.Param(PAGE_ID_PARAM, model.Id);

      return route;
    }


    /// <inheritdoc />
    public override async Task<IRouteModel> Model(RoutingContext context, RouteResponse response)
    {
      Route route = response.Route;
      Page page = await context.Session.LoadAsync<Page>(route.ReferenceId);
      PageRoute resolved = new(route);
      resolved.Page = page;
      resolved.Parents = await GetParents(context, page);
      resolved.PageType = route.Param<string>(PAGE_TYPE_PARAM);

      return resolved;
    }


    /// <inheritdoc />
    public override async IAsyncEnumerable<Route> Seed(RoutingContext context)
    {
      var stream = await context.Session.Advanced.StreamAsync(context.Session.Query<Page>());
      while (await stream.MoveNextAsync())
      {
        yield return await Create(context, stream.Current.Document);
      }
    }


    /// <summary>
    /// Get parents for a page
    /// </summary>
    protected virtual async Task<IList<Page>> GetParents(RoutingContext context, Page model)
    {
      Pages_ByHierarchy.Result result = await context.Session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
        .ProjectInto<Pages_ByHierarchy.Result>()
        .Include<Pages_ByHierarchy.Result, Page>(x => x.Path.Select(p => p.Id))
        .FirstOrDefaultAsync(x => x.Id == model.Id);

      return (await context.Session.LoadAsync<Page>(result.Path.Select(x => x.Id))).Select(x => x.Value).ToList();
    }
  }
}
