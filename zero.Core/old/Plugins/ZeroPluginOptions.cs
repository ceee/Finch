using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zero.Core.Plugins
{
  public class ZeroPluginOptions : IZeroPluginOptions
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public List<string> LocalizationPaths { get; private set; } = new List<string>();

    public string PluginPath { get; set; }
  }


  public interface IZeroPluginOptions
  {
    string Name { get; set; }

    string Description { get; set; }

    List<string> LocalizationPaths { get; }

    string PluginPath { get; set; }
  }


  public interface IZeroPluginStartup
  {
    Task Startup();
  }
}
