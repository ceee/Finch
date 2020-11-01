using System.Collections.Generic;
using zero.Core.Plugins;

namespace Zero.Web.DevServer
{
  public class ZeroPluginManager
  {
    protected IEnumerable<IZeroPlugin> Plugins { get; set; }


    public ZeroPluginManager(IEnumerable<IZeroPlugin> plugins)
    {
      Plugins = plugins;
    }
  }
}
