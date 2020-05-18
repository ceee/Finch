using Microsoft.Extensions.DependencyInjection;
using zero.Core.Options;

namespace zero.Core.Plugins
{
  public interface IZeroPlugin
  {
    void ConfigureServices(IServiceCollection services);

    void Configure(IZeroOptions zero);
  }
}