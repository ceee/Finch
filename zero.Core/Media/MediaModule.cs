using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace zero.Media;

public class MediaModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IMediaFileSystem, MediaFileSystem>(svc =>
    {
      IOptions<MediaOptions> options = svc.GetRequiredService<IOptions<MediaOptions>>();
      IWebHostEnvironment env = svc.GetRequiredService<IWebHostEnvironment>();
      return new(Path.Combine(env.WebRootPath, options.Value.FolderPath), options.Value.PublicPathPrefix);
    });

    services.AddScoped<IMediaStore, MediaStore>();
    services.AddScoped<IMediaCreator, MediaCreator>();
    services.AddScoped<IMediaManagement, MediaManagement>();

    services.AddOptions<MediaOptions>().Bind(configuration.GetSection("Zero:Media")).Configure(opts =>
    {
      opts.FolderPath = "uploads";
      opts.AllowedOtherFileExtensions = new() { ".pdf" };
      opts.AllowedImageFileExtensions = new() { ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".gif" };
      opts.Thumbnails = new()
      {
        { "thumb", new ResizeOptions() { Size = new(100, 100), Mode = ResizeMode.Max } },
        { "preview", new ResizeOptions() { Size = new(210, 210), Mode = ResizeMode.Min } }
      };
    });

    services.Configure<ZeroOptions>(opts =>
    {
      RavenOptions raven = opts.For<RavenOptions>();
      raven.Indexes.Add<Media_ByChildren>();
      raven.Indexes.Add<Media_ByParent>();
      raven.Indexes.Add<Media_ByHierarchy>();
    });

    //services.Configure<FlavorOptions>(opts =>
    //{
    //  // TODO should we use MediaType here?
    //  opts.Provide<Media>("zero.media", "@media.name", icon: "fth-image");
    //});
  }
}