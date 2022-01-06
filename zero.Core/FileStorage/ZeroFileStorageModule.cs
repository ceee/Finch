using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.FileStorage;

internal class ZeroFileStorageModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IPaths>(factory => new Paths(factory.GetService<IWebHostEnvironment>()));

    services.AddOptions<FileSystemOptions>().Bind(configuration.GetSection("Zero:FileSystem")).Configure(opts =>
    {
      opts.ZeroAssetsPath = "zero";
    });
  }
}