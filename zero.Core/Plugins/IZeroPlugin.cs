using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using zero.Core.Options;

namespace zero.Core.Plugins
{
  public abstract class ZeroPlugin
  {
    public string Name { get; protected set; }

    public string Description { get; set; }

    public virtual void ConfigureServices(IServiceCollection services) { }

    public virtual void Configure(IZeroPluginOptions plugin, IZeroOptions zero) { }
  }


  public interface IZeroPlugin
  {
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);

    void Configure(IZeroPluginOptions plugin, IZeroOptions zero);
  }
}