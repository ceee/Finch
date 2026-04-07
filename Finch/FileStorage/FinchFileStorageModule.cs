using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Finch.FileStorage;

internal class FinchFileStorageModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IPaths>(factory => new Paths(factory.GetService<IWebHostEnvironment>()));

    services.AddOptions<FileSystemOptions>().Bind(configuration.GetSection("Finch:FileSystem")).Configure(opts =>
    {
      opts.FinchAssetsPath = "finch";
    });

    services.AddSingleton<IWebRootFileSystem, WebRootFileSystem>(svc =>
    {
      IWebHostEnvironment env = svc.GetRequiredService<IWebHostEnvironment>();
      return new(env.WebRootPath, null);
    });
  }
}