namespace zero.Preview;

public class PreviewRouteProvider : ZeroRouteProvider<PreviewRouteModel>
{
  public PreviewRouteProvider() : base("zero.preview") { }


  /// <inheritdoc />
  public override Task<bool> IsRouteStale(RoutingContext context, PreviewRouteModel previous, PreviewRouteModel current) => Task.FromResult(false);

  /// <inheritdoc />
  public override async Task<Route> Create(RoutingContext context, PreviewRouteModel model)
  {
    Route route = await base.Create(context, model);
    route.Url = context.Context.Options.For<PreviewOptions>().PreviewPath.EnsureStartsWith('/').TrimEnd('/');
    return route;
  }

  /// <inheritdoc />
  public override async IAsyncEnumerable<Route> Seed(RoutingContext context)
  {
    yield return await Create(context, new PreviewRouteModel()
    {
      Id = String.Empty,
      Hash = String.Empty
    });
  }

  /// <inheritdoc />
  public override Task<IRouteModel> Model(RoutingContext context, RouteResponse response)
  {
    return Task.FromResult<IRouteModel>(new PreviewRoute()
    {
      Route = response.Route
    });
  }

  /// <inheritdoc />
  public override IRouteEndpoint Map(RoutingContext context, IRouteModel route)
  {
    return base.Map(context, route);
  }
}