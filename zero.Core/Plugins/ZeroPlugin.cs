using Microsoft.Extensions.DependencyInjection;

namespace zero.Core.Plugins
{
  public abstract class ZeroPlugin : IZeroPlugin
  {
    public virtual void Configure(IServiceCollection services, IZeroPluginConfiguration zero) { }

  }


  public interface IZeroPlugin
  {
    void Configure(IServiceCollection services, IZeroPluginConfiguration zero);
  }
}