using Microsoft.AspNetCore.Builder;

namespace zero;

public static class ZeroApplicationBuilderExtensions
{
  public static IZeroApplicationBuilder UseZero(this IApplicationBuilder app) => new ZeroApplicationBuilder(app);
}
