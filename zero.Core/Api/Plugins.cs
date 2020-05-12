using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zero.Core.Plugins;

namespace zero.Core.Api
{
  public class Plugins : ZeroPlugin, IPlugins
  {
    IEnumerable<ZeroPlugin> Items;


    public Plugins(IEnumerable<ZeroPlugin> plugins)
    {
      Items = plugins;

      foreach (ZeroPlugin plugin in plugins)
      {
        Spaces.AddRange(plugin.Spaces);
        Features.AddRange(plugin.Features);
        PageTypes.AddRange(plugin.PageTypes);
        Permissions.AddRange(plugin.Permissions);
        Renderers.AddRange(plugin.Renderers);
        Sections.AddRange(plugin.Sections);
      }
    }
  }


  public interface IPlugins
  {

  }
}
