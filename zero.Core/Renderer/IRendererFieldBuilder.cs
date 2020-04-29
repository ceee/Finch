using System;

namespace zero.Core.Renderer
{
  public interface IRendererFieldBuilder
  {
    IRendererFieldBuilder Required();

    IRendererFieldBuilder Text();

    IRendererFieldBuilder Textarea();

    IRendererFieldBuilder Rte();

    IRendererFieldBuilder IconPicker();

    IRendererFieldBuilder Toggle();

    IRendererFieldBuilder State(Action<StateOptions> optionsBuilder = null);

    IRendererFieldBuilder Media(Action<MediaOptions> optionsBuilder = null);

    IRendererFieldBuilder Custom(string path, Func<object> optionsBuilder = null);
  }
}
