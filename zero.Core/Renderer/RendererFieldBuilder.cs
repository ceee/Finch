using System;

namespace zero.Core.Renderer
{
  public class RendererFieldBuilder : IRendererFieldBuilder
  {
    string View = null;

    string ComponentPath = null;

    object Options = null;

    AbstractGenericRenderer CustomRenderer = null;


    internal class Data
    {
      public string View = null;

      public string ComponentPath = null;

      public object Options = null;

      public AbstractGenericRenderer Renderer = null;
    }

    internal Data Build()
    {
      return new Data()
      {
        View = View,
        ComponentPath = ComponentPath,
        Options = Options,
        Renderer = CustomRenderer
      };
    }


    T BuildOptions<T>(Action<T> builder) where T : new()
    {
      T options = new T();
      builder?.Invoke(options);
      return options;
    }

    public void Custom(string path, Func<object> optionsBuilder = null)
    {
      View = "custom";
      ComponentPath = path;
      Options = optionsBuilder?.Invoke();
    }

    public void IconPicker()
    {
      View = "iconPicker";
    }

    public void Media(Action<MediaOptions> optionsBuilder = null)
    {
      View = "media";
      Options = BuildOptions(optionsBuilder);
    }

    public void Nested<T>(IRenderer<T> renderer, Action<NestedOptions> optionsBuilder = null)
    {
      View = "nested";
      CustomRenderer = renderer.ToGenericRenderer();
      Options = BuildOptions(optionsBuilder);
    }

    public void Output()
    {
      View = "output";
    }

    public void Rte()
    {
      View = "rte";
    }

    public void Renderer<T>(IRenderer<T> renderer, Action<DefaultRendererOptions> optionsBuilder = null)
    {
      View = "_renderer";
      CustomRenderer = renderer.ToGenericRenderer();
      Options = BuildOptions(optionsBuilder);
    }

    public void State(Action<StateOptions> optionsBuilder = null)
    {
      View = "state";
      Options = BuildOptions(optionsBuilder);
    }

    public void Text(Action<TextOptions> optionsBuilder = null)
    {
      View = "text";
      Options = BuildOptions(optionsBuilder);
    }

    public void Textarea(Action<TextOptions> optionsBuilder = null)
    {
      View = "textarea";
      Options = BuildOptions(optionsBuilder);
    }

    public void Toggle()
    {
      View = "toggle";
    }
  }
}
