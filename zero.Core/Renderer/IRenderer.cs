using System.Threading.Tasks;

namespace zero.Core.Renderer
{
  public interface IRenderer<T>
  {
    RendererConfig Build();

    AbstractGenericRenderer ToGenericRenderer();
  }
}
