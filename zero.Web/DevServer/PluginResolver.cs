using System.Collections.Generic;
using zero.Core.Plugins;

namespace Zero.Web.DevServer
{
  public class PluginResolver
  {
    protected IEnumerable<IZeroPlugin> Plugins { get; set; }


    public PluginResolver(IEnumerable<IZeroPlugin> plugins)
    {
      Plugins = plugins;
    }
  }
}
