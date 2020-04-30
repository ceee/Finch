using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace zero.Core.Renderer
{
  internal class RenderProperty
  {
    public string Method { get; set; }

    public dynamic Params { get; set; }

    [JsonIgnore]
    public Action Builder { get; set; }

    [JsonIgnore]
    public Action<RenderProperty> Compile { get; set; }

    public List<RenderProperty> Children { get; set; } = new List<RenderProperty>();
  }
}
