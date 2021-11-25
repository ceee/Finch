using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace zero.Media;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroMedia(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<IMediaStore, MediaStore>();
    services.AddScoped<IMediaCreator, MediaCreator>();
    services.AddScoped<IMediaManagement, MediaManagement>();

    services.AddSingleton<IMediaFileSystem, MediaFileSystem>(svc =>
    {
      IOptions<MediaOptions> options = svc.GetRequiredService<IOptions<MediaOptions>>();
      IWebHostEnvironment env = svc.GetRequiredService<IWebHostEnvironment>();
      return new(Path.Combine(env.WebRootPath, options.Value.FolderPath), options.Value.PublicPathPrefix);
    });

    services.AddOptions<MediaOptions>().Bind(config.GetSection("Zero:Media")).Configure(opts =>
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
      raven.Indexes.Add<MediaFolder_ByHierarchy>();
      raven.Indexes.Add<MediaFolders_WithChildren>();
    });
    return services;
  }
}