using Microsoft.AspNetCore.Builder;
#if DEBUG
using ViteProxy;
#endif

namespace Mixtape.Frontend;

public static class MixtapeBuilderExtensions
{
  public static MixtapeBuilder AddVite(this MixtapeBuilder builder)
  {
#if DEBUG
    builder.Services.AddViteProxy("Mixtape:Vite");
#endif
    return builder;
  }
}

public static class ViteProxyApplicationBuilderExtensions
{
  public static IApplicationBuilder UseVite(this IApplicationBuilder app, string path = null, string configName = null)
  {
#if DEBUG
    app.UseViteStaticFiles(path, configName);
#endif
    return app;
  }
}

// internal class MixtapeViteModule : MixtapeModule
// {
//   public override int ConfigureOrder { get; } = -1;
//
//   public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
//   {
// #if DEBUG
//     services.AddViteProxy();
//
//     services.AddHttpClient("vite-proxy")
//       .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
//       {
//         UseCookies = false,
//         AllowAutoRedirect = false,
//         AutomaticDecompression = System.Net.DecompressionMethods.None,
//         PooledConnectionLifetime = TimeSpan.FromMinutes(5),
//         PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
//         MaxConnectionsPerServer = 256
//       });
// #endif
//   }
//
//
//   public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
//   {
// #if DEBUG
//     app.UseViteStaticFiles();
// #endif
//     //
// // app.Use(async (context, next) =>
// // {
// //   if (context.Request.Path.StartsWithSegments("/@viteproxy", out _))
// //   {
// //     var targetUri = new Uri($"http://localhost:5123{context.Request.Path}{context.Request.QueryString}");
// //
// //     var client = context.RequestServices.GetRequiredService<IHttpClientFactory>()
// //       .CreateClient("vite-proxy");
// //
// //     using var requestMessage = new HttpRequestMessage(new HttpMethod(context.Request.Method), targetUri);
// //
// //     if (context.Request.ContentLength > 0)
// //       requestMessage.Content = new StreamContent(context.Request.Body);
// //
// //     if (!string.IsNullOrEmpty(context.Request.ContentType) && requestMessage.Content != null)
// //       requestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", context.Request.ContentType);
// //
// //     using var responseMessage = await client.SendAsync(
// //       requestMessage,
// //       HttpCompletionOption.ResponseHeadersRead,
// //       context.RequestAborted);
// //
// //     context.Response.StatusCode = (int)responseMessage.StatusCode;
// //
// //     foreach (var header in responseMessage.Headers)
// //       context.Response.Headers[header.Key] = header.Value.ToArray();
// //
// //     foreach (var header in responseMessage.Content.Headers)
// //       context.Response.Headers[header.Key] = header.Value.ToArray();
// //
// //     context.Response.Headers.Remove("transfer-encoding");
// //     context.Response.Headers.Remove("connection");
// //
// //     await responseMessage.Content.CopyToAsync(context.Response.Body, context.RequestAborted);
// //     return;
// //   }
// //
// //   await next();
// // });
//   }
// }