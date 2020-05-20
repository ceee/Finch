using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace zero.Core.Renderer
{
  public class RenderProperty
  {
    public string Method { get; set; }

    public dynamic Params { get; set; }

    public Action Builder { get; set; }

    public Action<RenderProperty> Compile { get; set; }

    public AbstractGenericRenderer NestedRenderer { get; set; }

    public List<RenderProperty> Children { get; set; } = new List<RenderProperty>();
  }
}
