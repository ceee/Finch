using Microsoft.AspNetCore.Builder;
using System;
using zero.Web.Middlewares;

namespace zero.Web
{
  public static class ZeroApplicationBuilderExtensions
  {
    public static IZeroApplicationBuilder UseZero(this IApplicationBuilder app) => new ZeroApplicationBuilder(app);
  }
}
