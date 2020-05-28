using System.Collections.Generic;

namespace zero.Core.Renderer
{
  public abstract class AbstractFieldOptions
  {
    public bool HideLabel { get; set; }

    public List<string> Classes { get; set; } = new List<string>();

    public string HelpText { get; set; }
  }
}
