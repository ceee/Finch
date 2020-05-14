using Microsoft.Extensions.DependencyInjection;
using zero.Core.Options;

namespace zero.Core.Plugins
{
  //public abstract class ZeroPlugin : IZeroPlugin
  //{
  //  public virtual void Configure(IServiceCollection services, IZeroOptions zero) { }

  //}


  public interface IZeroPlugin
  {
    void Configure(IServiceCollection services, IZeroOptions zero);
  }
}