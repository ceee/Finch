using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace zero.Context;

public class ZeroContext : IZeroContext
{
  /// <inheritdoc />
  public IZeroOptions Options { get; protected set; }
  /// <inheritdoc />
  public IServiceProvider Services { get; private set; }

  protected ICultureResolver CultureResolver { get; private set; }

  protected ILogger<ZeroContext> Logger { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected IHttpContextAccessor HttpContextAccessor { get; private set; }

  protected IPrimitiveTypeCollection ValueCollection { get; private set; }


  bool _resolved = false;


  public ZeroContext(IZeroOptions options, IHttpContextAccessor httpContextAccessor, ICultureResolver cultureResolver, 
    ILogger<ZeroContext> logger, IHandlerHolder handler, IServiceProvider services)
  {
    Options = options;
    CultureResolver = cultureResolver;
    Logger = logger;
    Handler = handler;
    ValueCollection = new PrimitiveTypeCollection();
    HttpContextAccessor = httpContextAccessor;
    Services = services;
  }


  /// <inheritdoc />
  public virtual async Task Resolve(HttpContext context)
  {
    if (_resolved)
    {
      return;
    }

    _resolved = true;

    // set current culture
    await CultureResolver.Resolve(this);
  }


  /// <inheritdoc />
  public T Get<T>() => ValueCollection.Get<T>();


  /// <inheritdoc />
  public void Set<T>(T value) => ValueCollection.Set(value);


  /// <inheritdoc />
  public void Remove<T>() => ValueCollection.Remove<T>();
}



public interface IZeroContext
{
  /// <summary>
  /// Global zero options
  /// </summary>
  IZeroOptions Options { get; }

  /// <summary>
  /// Service container
  /// </summary>
  IServiceProvider Services { get; }

  /// <summary>
  /// Resolves the current application (for backoffice + frontend requests) and
  /// the currently active backoffice user, as users are not signed in with the default scheme and do therefore not populate HttpContext.User
  /// </summary>
  Task Resolve(HttpContext context);

  /// <summary>
  /// Get a custom property from this scoped context
  /// </summary>
  T Get<T>();

  /// <summary>
  /// Add a custom property to this scoped context
  /// </summary>
  void Set<T>(T value);

  /// <summary>
  /// Remove a custom property from this scoped context
  /// </summary>
  void Remove<T>();
}