using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace zero.Core.Renderer
{
  public abstract class AbstractRenderer<T> : IRenderer<T> where T : new()
  {
    const string METHOD_FIELD = "field";

    const string METHOD_TAB = "tab";

    const string METHOD_BOX = "box";

    List<RenderProperty> Properties = new List<RenderProperty>();

    RenderProperty ParentProperty = null;

    int CurrentDepth = 0;


    protected string LabelTemplate = null;

    protected string DescriptionTemplate = null;

    protected IValidator<T> Validator = null;


    public async Task<RendererConfig> Build()
    {
      RendererConfig config = new RendererConfig();
      config.Type = typeof(T);

      await Task.Delay(0);

      // compile fields
      static void compile(List<RenderProperty> properties)
      {
        foreach (RenderProperty property in properties)
        {
          property.Compile?.Invoke(property);

          if (property.Children.Count > 0)
          {
            compile(property.Children);
          }
        }
      }
      compile(Properties);

      // move top-level properties into tab in case other tabs exist
      int countTabs = Properties.Count(x => x.Method == METHOD_TAB);

      if (countTabs > 0 && countTabs < Properties.Count)
      {
        int index = Properties.FindIndex(x => x.Method != METHOD_TAB);
        List<RenderProperty> children = Properties.Where(x => x.Method != METHOD_TAB).ToList();

        Properties.Insert(index, new RenderProperty()
        {
          Method = METHOD_TAB,
          Children = children,
          Params = new
          {
            Name = "@ui.tab_general"
          }
        });

        Properties = Properties.Where(x => x.Method == METHOD_TAB).ToList();
      }

      // map to result
      static RendererComponent Map(RenderProperty property)
      {
        return new RendererComponent()
        {
          Method = property.Method,
          Params = property.Params,
          Components = property.Children.Select(x => Map(x)).ToList()
        };
      }

      config.Components = Properties.Select(x => Map(x)).ToList();

      return config;
    }


    protected virtual IRendererFieldBuilder Field(Expression<Func<T, object>> mapExpression, string label = null, string description = null, bool required = false)
    {
      RendererFieldBuilder builder = new RendererFieldBuilder();

      Add(new RenderProperty()
      {
        Method = METHOD_FIELD,
        Compile = property =>
        {
          object field = mapExpression.Compile().Invoke(new T());

          RendererFieldBuilder.Data fieldData = builder.Build();

          property.Params = new
          {
            Field = field,
            View = fieldData.View,
            ComponentPath = fieldData.ComponentPath,
            Options = fieldData.Options ?? new List<string>() { },
            Label = label,
            Description = description,
            Required = required
          };
        }
      });

      return builder;
    }

    protected virtual IRenderer<T> Tab(string name, Action builder)
    {
      Add(new RenderProperty()
      {
        Method = METHOD_TAB,
        Builder = builder,
        Params = new
        {
          Name = name
        }
      });

      return this;
    }

    protected virtual IRenderer<T> Box(string name, string description, Action builder)
    {
      Add(new RenderProperty()
      {
        Method = METHOD_BOX,
        Builder = builder,
        Params = new
        {
          Name = name,
          Description = description
        }
      });

      return this;
    }

    private void Add(RenderProperty property)
    {
      if (property.Method == METHOD_TAB && CurrentDepth > 0)
      {
        throw new Exception("Tabs have to be defined on the root level of the renderer");
      }
      if (property.Method == METHOD_BOX && (CurrentDepth > 1 || (CurrentDepth == 1 && ParentProperty?.Method != METHOD_TAB)))
      {
        throw new Exception("Boxes have to be defined on the root level of the renderer or as direct descendents of a tab");
      }

      if (CurrentDepth == 0)
      {
        Properties.Add(property);
      }
      else
      {
        ParentProperty.Children.Add(property);
      }

      if (property.Builder != null)
      {
        CurrentDepth += 1;

        RenderProperty previousParent = ParentProperty;

        ParentProperty = property;

        property.Builder();

        CurrentDepth -= 1;
        ParentProperty = previousParent;
      }
    }
  }
}
