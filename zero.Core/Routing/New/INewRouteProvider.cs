using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public abstract class ZeroRouteProvider<T> : ZeroRouteProvider, INewRouteProvider<T> where T : IZeroRouteEntity
  {
    public ZeroRouteProvider(string alias) : base(alias) { }

    /// <inheritdoc />
    public override bool CanHandle(Type type) => typeof(T).IsAssignableFrom(type);

    /// <inheritdoc />
    public abstract Task<Route> Create(RoutingContext context, T model);

    /// <inheritdoc />
    public sealed override Task<Route> Create(RoutingContext context, IZeroRouteEntity model) => Create(context, (T)model);

    /// <inheritdoc />
    public virtual string Key(T model) => model.Hash;

    /// <inheritdoc />
    public sealed override string Key(IZeroRouteEntity model) => Key((T)model);

    /// <inheritdoc />
    public virtual Task<IRouteModel> Model(RoutingContext context, Route route, T entity) => Task.FromResult((IRouteModel)new RouteModel() { Route = route });

    /// <inheritdoc />
    public sealed override Task<IRouteModel> Model(RoutingContext context, Route route, IZeroRouteEntity entity) => Model(context, route, (T)entity);
  }


  public abstract class ZeroRouteProvider : INewRouteProvider
  {
    public ZeroRouteProvider(string alias)
    {
      Alias = alias;
    }

    public virtual string Alias { get; protected set; }

    /// <inheritdoc />
    public virtual bool CanHandle(Type type) => false;

    /// <inheritdoc />
    public abstract Task<Route> Create(RoutingContext context, IZeroRouteEntity model);

    /// <inheritdoc />
    public virtual string Key(IZeroRouteEntity model) => model.Hash;

    /// <inheritdoc />
    public virtual IAsyncEnumerable<Route> Seed(RoutingContext context) => AsyncEnumerable.Empty<Route>();

    /// <inheritdoc />
    public virtual Task<IRouteModel> Model(RoutingContext context, Route route, IZeroRouteEntity entity) => Task.FromResult((IRouteModel)new RouteModel() { Route = route });
  }


  public interface INewRouteProvider<T> : INewRouteProvider where T : IZeroRouteEntity
  {
    /// <summary>
    /// Generate unique route key for a model
    /// </summary>
    string Key(T model);

    /// <summary>
    /// Create route entity from a model
    /// </summary>
    Task<Route> Create(RoutingContext context, T model);

    /// <summary>
    /// Converts a route to a model which is passed to the endpoint
    /// </summary>
    Task<IRouteModel> Model(RoutingContext context, Route route, T entity);
  }


  public interface INewRouteProvider
  {
    /// <summary>
    /// Alias of this route provider
    /// </summary>
    string Alias { get; }

    /// <summary>
    /// Whether this provider can handle a certain entity type
    /// </summary>
    bool CanHandle(Type type);

    /// <summary>
    /// Generate unique route key for a model
    /// </summary>
    string Key(IZeroRouteEntity model);

    /// <summary>
    /// Create route entity from a model
    /// </summary>
    Task<Route> Create(RoutingContext context, IZeroRouteEntity model);

    /// <summary>
    /// Get all models which should be provided and handled by this instance
    /// </summary>
    IAsyncEnumerable<Route> Seed(RoutingContext context);

    /// <summary>
    /// Converts a route to a model which is passed to the endpoint
    /// </summary>
    Task<IRouteModel> Model(RoutingContext context, Route route, IZeroRouteEntity entity);
  }

}
