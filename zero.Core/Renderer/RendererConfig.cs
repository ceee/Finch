using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace zero.Core.Renderer
{
  public class RendererConfig
  {
    public Type Type { get; set; }

    public List<RendererComponent> Components { get; set; } = new List<RendererComponent>();
  }


  public class RendererComponent
  {
    public string Method { get; set; }

    public dynamic Params { get; set; }

    public List<RendererComponent> Components { get; set; } = new List<RendererComponent>();
  }
}
