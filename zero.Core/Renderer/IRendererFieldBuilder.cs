using System;

namespace zero.Core.Renderer
{
  public interface IRendererFieldBuilder
  {
    IRendererFieldBuilder Required();

    IRendererFieldBuilder Text(Action<TextOptions> optionsBuilder = null);

    IRendererFieldBuilder Textarea(Action<TextOptions> optionsBuilder = null);

    IRendererFieldBuilder Rte();

    IRendererFieldBuilder IconPicker();

    IRendererFieldBuilder Toggle();

    IRendererFieldBuilder State(Action<StateOptions> optionsBuilder = null);

    IRendererFieldBuilder Media(Action<MediaOptions> optionsBuilder = null);

    IRendererFieldBuilder Output();

    IRendererFieldBuilder Nested<T>(IRenderer<T> renderer, Action<NestedOptions> optionsBuilder = null);

    IRendererFieldBuilder Custom(string path, Func<object> optionsBuilder = null);

  }
}
