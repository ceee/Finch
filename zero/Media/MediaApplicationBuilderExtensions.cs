using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;

namespace zero.Media;

public static class MediaApplicationBuilderExtensions
{
  public static IApplicationBuilder UseStaticFilesWithCache(this IApplicationBuilder app, TimeSpan cacheDuration)
  {
    return app.UseStaticFiles(new StaticFileOptions
    {
      OnPrepareResponse = ctx =>
      {
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public, must-revalidate, max-age=" + (int)cacheDuration.TotalSeconds;
      },
      OnPrepareResponseAsync = async ctx =>
      {
        await Task.Delay(0);
      }
    });
  }
}