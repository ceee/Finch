namespace zero.Routing;

public abstract class ZeroRouteProvider<T> : ZeroRouteProvider, IRouteProvider<T> where T : ISupportsRouting
{
  public ZeroRouteProvider(string alias) : base(alias) { }

  /// <inheritdoc />
  public override bool CanHandle(Type type) => typeof(T).IsAssignableFrom(type);

  /// <inheritdoc />
  public override bool CanSeed(Type type) => CanHandle(type);

  /// <inheritdoc />
  public virtual Task<Route> Create(RoutingContext context, T model) => base.Create(context, model);

  /// <inheritdoc />
  public sealed override Task<Route> Create(RoutingContext context, ISupportsRouting model) => Create(context, (T)model);

  /// <inheritdoc />
  public virtual string Key(T model) => model.Hash;

  /// <inheritdoc />
  public sealed override string Key(ISupportsRouting model) => Key((T)model);

  /// <inheritdoc />
  public virtual Task<bool> IsRouteStale(RoutingContext context, T previous, T current) => Task.FromResult(true);

  /// <inheritdoc />
  public sealed override Task<bool> IsRouteStale(RoutingContext context, ISupportsRouting previous, ISupportsRouting current) => IsRouteStale(context, (T)previous, (T)current);

  /// <inheritdoc />
  public virtual string Id(T model) => base.Id(model);

  /// <inheritdoc />
  public sealed override string Id(ISupportsRouting model) => base.Id(model);

  /// <inheritdoc />
  public virtual async Task<Route> Find(RoutingContext context, T model) => await context.Session.LoadAsync<Route>(Id(model));

  /// <inheritdoc />
  public sealed override Task<Route> Find(RoutingContext context, ISupportsRouting model) => Find(context, (T)model);

  /// <inheritdoc />
  public virtual async Task<Dictionary<T, Route>> Find(RoutingContext context, params T[] models)
  {
    Dictionary<string, T> idMap = models.ToDistinctDictionary(x => Id(x), x => x);
    Dictionary<string, Route> routes = await context.Session.LoadAsync<Route>(idMap.Select(x => x.Key));
    Dictionary<T, Route> result = new();

    foreach ((string id, T model) in idMap)
    {
      result.Add(model, routes.GetValueOrDefault(id));
    }

    return result;
  }

  /// <inheritdoc />
  public sealed override async Task<Dictionary<ISupportsRouting, Route>> Find(RoutingContext context, params ISupportsRouting[] models)
  {
    return (await Find(context, models.Select(x => (T)x).ToArray())).ToDictionary(x => (ISupportsRouting)x.Key, x => x.Value);
  }
}


public abstract class ZeroRouteProvider : IRouteProvider
{
  protected static string ID_PARAM = "id";

  protected PageRouteResolverHelper PageResolver { get; set; } = new();


  public ZeroRouteProvider(string alias)
  {
    Alias = alias;
  }

  public virtual string Alias { get; protected set; }

  /// <inheritdoc />
  public uint Priority { get; protected set; } = 0;

  /// <inheritdoc />
  public virtual bool CanHandle(Type type) => false;

  /// <inheritdoc />
  public virtual bool CanSeed(Type type) => false;

  /// <inheritdoc />
  public virtual Task<Route> Create(RoutingContext context, ISupportsRouting model) => Task.FromResult(new Route()
  {
    Id = Id(model),
    ProviderAlias = Alias,
    ReferenceId = model.Id
  });

  /// <inheritdoc />
  public virtual string Key(ISupportsRouting model) => model.Hash;

  /// <inheritdoc />
  public virtual IAsyncEnumerable<Route> Seed(RoutingContext context) => AsyncEnumerable.Empty<Route>();

  /// <inheritdoc />
  public virtual IAsyncEnumerable<Route> SeedOnUpdate<T>(RoutingContext context, T model) where T : ISupportsRouting => AsyncEnumerable.Empty<Route>();

  /// <inheritdoc />
  public virtual Task<IRouteModel> Model(RoutingContext context, RouteResponse response) => Task.FromResult((IRouteModel)new RouteModel() { Route = response.Route });

  /// <inheritdoc />
  public virtual Task<bool> IsRouteStale(RoutingContext context, ISupportsRouting previous, ISupportsRouting current) => Task.FromResult(true);

  /// <inheritdoc />
  public virtual string Id(ISupportsRouting model) => "routes." + Alias + "." + Key(model);

  /// <inheritdoc />
  public virtual IRouteEndpoint Map(RoutingContext context, IRouteModel route)
  {
    RoutingOptions options = context.Context.Options.For<RoutingOptions>();

    IEnumerable<Func<IRouteModel, IRouteEndpoint>> resolvers = options.EndpointResolvers.GetAll(route.GetType());

    foreach (Func<IRouteModel, IRouteEndpoint> resolver in resolvers.Reverse())
    {
      IRouteEndpoint endpoint = resolver(route);

      if (endpoint != null)
      {
        return endpoint;
      }
    }

    return options.DefaultEndpoint;
  }

  /// <inheritdoc />
  public virtual async Task<Route> Find(RoutingContext context, ISupportsRouting model) => await context.Session.LoadAsync<Route>(Id(model));

  /// <inheritdoc />
  public virtual async Task<Dictionary<ISupportsRouting, Route>> Find(RoutingContext context, params ISupportsRouting[] models)
  {
    Dictionary<string, ISupportsRouting> idMap = models.ToDistinctDictionary(x => Id(x), x => x);
    Dictionary<string, Route> routes = await context.Session.LoadAsync<Route>(idMap.Select(x => x.Key));
    Dictionary<ISupportsRouting, Route> result = new();

    foreach ((string id, ISupportsRouting model) in idMap)
    {
      result.Add(model, routes.GetValueOrDefault(id));
    }

    return result;
  }
}


public interface IRouteProvider<T> : IRouteProvider where T : ISupportsRouting
{
  /// <summary>
  /// Generate unique route key for a model
  /// </summary>
  string Key(T model);

  /// <summary>
  /// Generate unique ID for a model
  /// </summary>
  string Id(T model);

  /// <summary>
  /// Create route entity from a model
  /// </summary>
  Task<Route> Create(RoutingContext context, T model);

  /// <summary>
  /// Determines whether the route for previous is stale and needs to be refreshed 
  /// based on comparison with the previous version
  /// </summary>
  Task<bool> IsRouteStale(RoutingContext context, T previous, T current);

  /// <summary>
  /// Find a persisted route for an entity
  /// </summary>
  Task<Route> Find(RoutingContext context, T model);
}


public interface IRouteProvider
{
  /// <summary>
  /// Alias of this route provider
  /// </summary>
  string Alias { get; }

  /// <summary>
  /// Providers with higher priority will run before other providers when seeding
  /// </summary>
  uint Priority { get; }

  /// <summary>
  /// Whether this provider can handle a certain entity type
  /// </summary>
  bool CanHandle(Type type);

  /// <summary>
  /// Whether this provider can handle seeding for a certain entity type
  /// </summary>
  bool CanSeed(Type type);

  /// <summary>
  /// Generate unique route key for a model
  /// </summary>
  string Key(ISupportsRouting model);

  /// <summary>
  /// Generate unique ID for a model
  /// </summary>
  string Id(ISupportsRouting model);

  /// <summary>
  /// Create route entity from a model
  /// </summary>
  Task<Route> Create(RoutingContext context, ISupportsRouting model);

  /// <summary>
  /// Get all models which should be provided and handled by this instance
  /// </summary>
  IAsyncEnumerable<Route> Seed(RoutingContext context);

  /// <summary>
  /// Get all models which should be updated when an entity changes
  /// </summary>
  IAsyncEnumerable<Route> SeedOnUpdate<T>(RoutingContext context, T model) where T : ISupportsRouting;

  /// <summary>
  /// Converts a route to a model which is passed to the endpoint
  /// </summary>
  Task<IRouteModel> Model(RoutingContext context, RouteResponse response);

  /// <summary>
  /// Determines whether the route for previous is stale and needs to be refreshed 
  /// based on comparison with the previous version
  /// </summary>
  Task<bool> IsRouteStale(RoutingContext context, ISupportsRouting previous, ISupportsRouting current);

  /// <summary>
  /// Map a route model to an endpoint
  /// </summary>
  IRouteEndpoint Map(RoutingContext context, IRouteModel route);

  /// <summary>
  /// Find a persisted route for an entity
  /// </summary>
  Task<Route> Find(RoutingContext context, ISupportsRouting model);

  /// <summary>
  /// Find persisted routes for entities
  /// </summary>
  Task<Dictionary<ISupportsRouting, Route>> Find(RoutingContext context, params ISupportsRouting[] models);
}