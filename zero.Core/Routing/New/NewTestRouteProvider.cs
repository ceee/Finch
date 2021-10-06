using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Routing
{
  public class NewRouteProvider : NewAbtractRouteProvider<Country>
  {
    protected Route PageRoute { get; set; }


    public NewRouteProvider(IZeroDocumentSession session, IZeroOptions options) : base("new.test", session, options)
    {
      Options = options;
    }


    /// <inheritdoc />
    public override async Task Warmup()
    {
      PageRoute ??= await ResolvePageRoute();
    }


    /// <inheritdoc />
    public override string Url(Country model)
    {
      return String.Join(SLASH, PageRoute.Url, "story", model.Alias);
    }


    /// <inheritdoc />
    public override Route Create(Country model)
    {
      return new Route(Alias)
      {
        Id = Id(model),
        Url = Url(model),
      }.DependsOn(model.Id);
    }


    /// <inheritdoc />
    public override async Task<IResolvedRoute> Resolve(RouteResponse response)
    {
      PageRoute resolved = new(response.Route);
      resolved.Page = await Session.LoadAsync<Page>(response.Route.Dependencies[0]);
      return resolved.Page == null ? null : resolved;
    }


    /// <inheritdoc />
    public override async Task<IEnumerable<Country>> All()
    {
      return await Session.Query<Country>().ToListAsync();
    }
  }
}
