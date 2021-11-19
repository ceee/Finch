using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using zero.Core.Options;

namespace zero.Core.Plugins
{
  public abstract class ZeroPlugin : IZeroPlugin
  {
    public IZeroPluginOptions Options { get; } = new ZeroPluginOptions();

    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration) { }

    public virtual void Configure(IZeroOptions zero) { }
  }


  public interface IZeroPlugin
  {
    IZeroPluginOptions Options { get; }

    void ConfigureServices(IServiceCollection services, IConfiguration configuration);

    void Configure(IZeroOptions zero);
  }
}