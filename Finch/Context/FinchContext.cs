using Microsoft.AspNetCore.Http;

namespace Finch.Context;

public class FinchContext(
  IFinchOptions options,
  ICultureResolver cultureResolver,
  IServiceProvider services)
  : IFinchContext
{
  /// <inheritdoc />
  public IFinchOptions Options { get; } = options;

  /// <inheritdoc />
  public IServiceProvider Services { get; } = services;


  bool _resolved = false;


  /// <inheritdoc />
  public virtual async Task Resolve(HttpContext context)
  {
    if (_resolved)
    {
      return;
    }

    // set current culture
    await cultureResolver.Resolve(this);

    _resolved = true;
  }
}



public interface IFinchContext
{
  /// <summary>
  /// Global finch options
  /// </summary>
  IFinchOptions Options { get; }

  /// <summary>
  /// Service container
  /// </summary>
  IServiceProvider Services { get; }

  /// <summary>
  /// Resolves the current application (for backoffice + frontend requests) and
  /// the currently active backoffice user, as users are not signed in with the default scheme and do therefore not populate HttpContext.User
  /// </summary>
  Task Resolve(HttpContext context);
}