namespace zero.Core.Renderer
{
  public interface IRenderer<T> : IRenderer { }


  public interface IRenderer
  {
    string Alias { get; }

    RendererConfig Build();

    AbstractGenericRenderer ToGenericRenderer();
  }
}
