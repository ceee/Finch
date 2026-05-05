using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.FileStorage;

internal class MixtapeFileStorageModule : MixtapeModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IPaths>(factory => new Paths(factory.GetService<IWebHostEnvironment>()));

    services.AddOptions<FileSystemOptions>().Bind(configuration.GetSection("Mixtape:FileSystem")).Configure(opts =>
    {
      opts.MixtapeAssetsPath = "mixtape";
    });

    services.AddSingleton<IWebRootFileSystem, WebRootFileSystem>(svc =>
    {
      IWebHostEnvironment env = svc.GetRequiredService<IWebHostEnvironment>();
      return new(env.WebRootPath, null);
    });
  }
}