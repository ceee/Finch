using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Architecture;

public class ZeroModuleConfiguration : IZeroModuleConfiguration
{
  public IServiceCollection Services { get; }

  public IConfiguration Configuration { get; }

  public ZeroModuleConfiguration(IServiceCollection servicse, IConfiguration configuration)
  {
    Services = servicse;
    Configuration = configuration;
  }
}


public interface IZeroModuleConfiguration
{
  IServiceCollection Services { get; }

  IConfiguration Configuration { get; }
}
