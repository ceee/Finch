using Microsoft.Extensions.DependencyInjection;

namespace zero.Media;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroMedia(this IServiceCollection services)
  {
    services.AddScoped<IMediaStore, MediaStore>();
    services.AddScoped<IMediaFolderStore, MediaFolderStore>();

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