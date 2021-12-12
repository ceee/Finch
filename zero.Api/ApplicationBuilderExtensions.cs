using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseZeroApi(this IApplicationBuilder builder)
  {
    return builder;
  }
}