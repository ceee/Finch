using System.Collections.Generic;

namespace zero.Core.Renderer
{
  public interface IRendererOptions
  {
    string ComponentPath { get; set; }

    IList<ConstructorTab> Tabs { get; set; }
  }
}
