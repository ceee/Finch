using zero.Core.Renderer;

namespace zero.Core.Options
{
  public class RendererOptions : ZeroBackofficeCollection<AbstractGenericRenderer>, IZeroCollectionOptions
  {
    public RendererOptions()
    {
      
    }


    public void Add<TRenderer>() where TRenderer : IRenderer, new()
    {
      Items.Add(new TRenderer().ToGenericRenderer());
    }


    public void Add<TRenderer>(TRenderer renderer) where TRenderer : IRenderer, new()
    {
      Items.Add(renderer.ToGenericRenderer());
    }
  }
}
