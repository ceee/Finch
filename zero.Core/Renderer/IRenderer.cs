namespace zero.Core.Renderer
{
  public interface IRenderer<T> : IRenderer { }


  public interface IRenderer
  {
    RendererConfig Build();

    AbstractGenericRenderer ToGenericRenderer();
  }
}
