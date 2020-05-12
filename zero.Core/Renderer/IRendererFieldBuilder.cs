using System;

namespace zero.Core.Renderer
{
  public interface IRendererFieldBuilder
  {
    void Text(Action<TextOptions> optionsBuilder = null);

    void Textarea(Action<TextOptions> optionsBuilder = null);

    void Rte();

    void IconPicker();

    void Toggle();

    void State(Action<StateOptions> optionsBuilder = null);

    void Media(Action<MediaOptions> optionsBuilder = null);

    void Output();

    void Nested<T>(IRenderer<T> renderer, Action<NestedOptions> optionsBuilder = null);

    void Renderer<T>(IRenderer<T> renderer, Action<RendererOptions> optionsBuilder = null);

    void Custom(string path, Func<object> optionsBuilder = null);
  }
}
