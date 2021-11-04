using Raven.Client.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database.Indexes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class TestPageRouteProvider : ZeroRouteProvider<Page>
  {
    public static string PAGE_TYPE_PARAM = "pageType";

    protected IPageUrlBuilder UrlBuilder { get; set; }


    public TestPageRouteProvider(IPageUrlBuilder urlBuilder) : base("zero.pages")
    {
      UrlBuilder = urlBuilder;
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

      route.Url = UrlBuilder.GetUrl(model, parents);
      route.Dependencies.Add(model.Id);
      route.Dependencies.AddRange(parents.Select(x => x.Id));

      route.Params.Add(PAGE_TYPE_PARAM, model.PageTypeAlias);

      return route;
    }


    /// <inheritdoc />
    public override async Task<IRouteModel> Model(RoutingContext context, Route route, Page entity)
    {
      PageRoute resolved = new(route);
      resolved.Page = entity;
      resolved.Parents = await GetParents(context, entity);

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
