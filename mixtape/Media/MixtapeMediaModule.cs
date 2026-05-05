using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.DependencyInjection;
using System.IO;
using Mixtape.Media.ImageSharp;
using Mixtape.Media.ImageSharp.Processors;
using Microsoft.Extensions.Hosting;

namespace Mixtape.Media;

internal class MixtapeMediaModule : MixtapeModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    // automatically add imaging config file
    // which is used for image resizing presets and media config
    if (configuration is ConfigurationManager configurationManager)
    {
      configurationManager.AddJsonFile("imaging.json", true, true);
      configurationManager.AddJsonFile("imaging.{ENVIRONMENT}.json", true, true);
      configurationManager.AddJsonFile("Config/imaging.json", true, true);
      configurationManager.AddJsonFile("Config/imaging.{ENVIRONMENT}.json", true, true);
    }

    services.AddImageSharp()
      .SetRequestParser<PresetRequestParser>()
      .ClearProviders()
      .AddProvider<PhysicalFileProvider>()
      .AddProcessor<RotateWebProcessor>()
      .AddProcessor<StripMetadataWebProcessor>()
      .AddProcessor<BlurWebProcessor>()
      .AddProcessor<LoopWebProcessor>();

    services.AddOptions<PhysicalFileSystemCacheOptions>()
      .Configure(opts =>
      {
        opts.CacheRootPath = Path.GetTempPath();
        opts.CacheFolder = ".imagesharpcache";
      })
      .PostConfigure((PhysicalFileSystemCacheOptions opts, IHostEnvironment env) =>
      {
        // override cache location for dev env
        if (env.IsDevelopment())
        {
          opts.CacheRootPath = Path.GetTempPath();
          opts.CacheFolder = ".imagesharpcache";
        }
      })
      .Bind(configuration.GetSection("Mixtape:Media:ImageSharp:Cache"));
    
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
    services.AddScoped<IMixtapeMediaStoreDbProvider, EmptyMixtapeMediaStoreDbProvider>();
    services.AddSingleton<IImageDimensionReader, ImageDimensionReader>();
    services.AddSingleton<MediaMetadataCache>();

    services.AddOptions<MediaOptions>().Configure(opts =>
    {
      opts.FolderPath = "media";
      opts.AllowedOtherFileExtensions = [".pdf", ".docx", ".doc", ".svg", ".mp4", ".webm", ".avif", ".av1", ".xlsx", ".xls", ".xml"];
      opts.AllowedImageFileExtensions = [".jpg", ".jpeg", ".png", ".bmp", ".webp", ".gif", ".avif"];
      // opts.Thumbnails = new()
      // {
      //   { "thumb", new ResizeOptions() { Size = new(100, 100), Mode = ResizeMode.Max } },
      //   { "preview", new ResizeOptions() { Size = new(210, 210), Mode = ResizeMode.Min } }
      // };
    }).Bind(configuration.GetSection("Mixtape:Media"));

    // services.Configure<MixtapeOptions>(opts =>
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