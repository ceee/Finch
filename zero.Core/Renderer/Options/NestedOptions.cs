using System;
using System.Collections.Generic;
using System.Text;

namespace zero.Core.Renderer
{
  public class NestedOptions : AbstractFieldOptions
  {
    public int Limit { get; set; } = 10;

    public string AddLabel { get; set; } = "@ui.add";
  }
}
