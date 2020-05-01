using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Renderer;

namespace zero.Core.Entities
{
  public class RendererCollection : List<AbstractGenericRenderer>
  {
    public void Add<T, TRenderer>() where TRenderer : IRenderer<T>, new()
    {
      Add(new TRenderer().ToGenericRenderer());
    }


    public void Add<T, TRenderer>(TRenderer renderer) where TRenderer : IRenderer<T>, new()
    {
      Add(renderer.ToGenericRenderer());
    }
  }
}
