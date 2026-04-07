using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.DependencyInjection;
using System.IO;
using Finch.Media.ImageSharp;
using Finch.Media.ImageSharp.Processors;

namespace Finch.Media;

internal class FinchMediaModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddImageSharp()
      .SetRequestParser<PresetRequestParser>()
      .ClearProviders()
      .AddProvider<PhysicalFileProvider>()
      .AddProcessor<RotateWebProcessor>()
      .AddProcessor<StripMetadataWebProcessor>()
      .AddProcessor<BlurWebProcessor>()
      .AddProcessor<LoopWebProcessor>();

    services.AddOptions<PhysicalFileSystemCacheOptions>().Configure(opts =>
    {
      opts.CacheRootPath = "~/";
      opts.CacheFolder = "cache";
    }).Bind(configuration.GetSection("Finch:Media:ImageSharp:Cache"));
    
    //configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Config/imaging.json"), true, true);
    //configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), $"Config/imaging.{builder.Environment.EnvironmentName}.json"), true);
    
    services.AddSingleton<IMediaFileSystem, MediaFileSystem>(svc =>
    {
      IOptions<MediaOptions> options = svc.GetRequiredService<IOptions<MediaOptions>>();
      IWebHostEnvironment env = svc.GetRequiredService<IWebHostEnvironment>();
      return new(Path.Combine(env.WebRootPath, options.Value.FolderPath), options.Value.PublicPathPrefix + options.Value.FolderPath.EnsureStartsWith('/'));
    });

    services.AddScoped<IMediaCreator, MediaCreator>();
    services.AddScoped<IStaticMediaCreator, StaticMediaCreator>();
    services.AddScoped<IMediaManagement, MediaManagement>();
    services.AddScoped<IFinchMediaStoreDbProvider, EmptyFinchMediaStoreDbProvider>();
    services.AddSingleton<IImageDimensionReader, ImageDimensionReader>();
    services.AddSingleton<MediaMetadataCache>();

    services.AddOptions<MediaOptions>().Configure(opts =>
    {
      opts.FolderPath = "media";
      opts.AllowedOtherFileExtensions = new() { ".pdf", ".docx", ".doc", ".svg", ".xml" };
      opts.AllowedImageFileExtensions = new() { ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".gif", ".avif" };
      // opts.Thumbnails = new()
      // {
      //   { "thumb", new ResizeOptions() { Size = new(100, 100), Mode = ResizeMode.Max } },
      //   { "preview", new ResizeOptions() { Size = new(210, 210), Mode = ResizeMode.Min } }
      // };
    }).Bind(configuration.GetSection("Finch:Media"));

    // services.Configure<FinchOptions>(opts =>
    // {
    //   RavenOptions raven = opts.For<RavenOptions>();
    //   raven.Indexes.Add<Media_ByChildren>();
    //   raven.Indexes.Add<Media_ByHierarchy>();
    // });
  }

  public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
  {
    app.UseImageSharp();
  }
}