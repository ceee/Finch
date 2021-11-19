using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero;

public class ZeroModuleConfiguration : IZeroModuleConfiguration
{
  public IServiceCollection Services { get; }

  public IConfiguration Configuration { get; }

  internal ZeroModuleConfiguration(IServiceCollection servicse, IConfiguration configuration)
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
