using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
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
    public static string PAGE_IS_FOLDER = "pageIsFolder";

    protected IPageUrlBuilder UrlBuilder { get; set; }


    public PageRouteProvider(IPageUrlBuilder urlBuilder) : base(Constants.Pages.PageRouteProviderAlias)
    {
      Priority = 10;
      UrlBuilder = urlBuilder;
    }


    /// <inheritdoc />
    public override Task<bool> IsRouteStale(RoutingContext context, Page previous, Page current)
    {
      bool compareUrl = UrlBuilder.GetUrlPart(previous) == UrlBuilder.GetUrlPart(current);
      bool compareParent = previous.ParentId == current.ParentId;
      return Task.FromResult(!compareUrl || !compareParent);
    }


    /// <inheritdoc />
    public override async Task<Route> Create(RoutingContext context, Page model)
    {
      IEnumerable<Page> parents = await GetParents(context, model);

      Route route = await base.Create(context, model);
      route.Url = UrlBuilder.GetUrl(model, parents);
      route.DependsOn(model.Id);
      route.DependsOn(parents.Select(x => x.Id).ToArray());
      route.Param(PAGE_TYPE_PARAM, model.PageTypeAlias);
      route.Param(PAGE_ID_PARAM, model.Id);
      route.Param(PAGE_IS_FOLDER, model is PageFolder);

      return route;
    }


    /// <inheritdoc />
    public override async Task<IRouteModel> Model(RoutingContext context, RouteResponse response)
    {
      Route route = response.Route;

      if (route.Param<bool>(PAGE_IS_FOLDER))
      {
        return null;
      }

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


    /// <inheritdoc />
    public override async IAsyncEnumerable<Route> SeedOnUpdate<T>(RoutingContext context, T model)
    {
      if (model is Page)
      {
        string id = model.Id;
        IList<Page> children = await context.Session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
          .Where(x => x.PathIds.In(new[] { id })).ProjectInto<Page>().ToListAsync();

        foreach (Page child in children)
        {
          yield return await Create(context, child);
        }
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
