using Microsoft.Extensions.DependencyInjection;
using zero.Core.Options;

namespace zero.Core.Plugins
{
  public interface IZeroPlugin
  {
    void Configure(IServiceCollection services, IZeroOptions zero);
  }
}