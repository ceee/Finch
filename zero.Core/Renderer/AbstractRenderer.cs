using FluentValidation;
using Lambda2Js;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using zero.Core.Extensions;

namespace zero.Core.Renderer
{
  public class AbstractGenericRenderer : AbstractRenderer<object>
  {
    public Type TargetType { get; set; }

    public AbstractGenericRenderer() { }

    internal AbstractGenericRenderer(Type type, string alias, List<RenderProperty> properties)
    {
      Alias = alias;
      TargetType = type;
      Properties = properties;
    }
  }


  public abstract class AbstractRenderer<T> : IRenderer<T>, IRenderer
  {
    const string METHOD_FIELD = "field";

    const string METHOD_TAB = "tab";

    const string METHOD_BOX = "box";

    protected List<RenderProperty> Properties { get; set; } = new List<RenderProperty>();

    RenderProperty ParentProperty = null;

    int CurrentDepth = 0;

    public string Alias { get; protected set; }

    protected string LabelTemplate = "{0}";

    protected string DescriptionTemplate = "{0}";

    protected Func<string, string> FindLabelName = field => "@" + field;

    protected Func<string, string> FindLabelDescriptionName = field => null;


    public RendererConfig Build()
    {
      RendererConfig config = new RendererConfig();
      config.Type = typeof(T);


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
        RendererComponent component = new RendererComponent()
        {
          Method = property.Method,
          Params = property.Params
        };

        if (property.Children.Count > 0)
        {
          component.Components = property.Children.Select(x => Map(x)).ToList();
        }
        else if (property.NestedRenderer != null)
        {
          component.Components = property.NestedRenderer.Build().Components;
        }

        return component;
      }

      config.Components = Properties.Select(x => Map(x)).ToList();


      return config;
    }


    public AbstractGenericRenderer ToGenericRenderer()
    {
      return new AbstractGenericRenderer(typeof(T), Alias, Properties);
    }


    protected virtual IRendererFieldBuilder Field(Expression<Func<T, object>> mapExpression, bool required = false)
    {
      RendererFieldBuilder builder = new RendererFieldBuilder();

      Add(new RenderProperty()
      {
        Method = METHOD_FIELD,
        Compile = property =>
        {
          string field = mapExpression.CompileToJavascript().ToCamelCaseId();

          RendererFieldBuilder.Data fieldData = builder.Build();

          property.NestedRenderer = fieldData.Renderer;
          property.Params = new
          {
            Field = field,
            View = fieldData.View,
            ComponentPath = fieldData.ComponentPath,
            Options = fieldData.Options ?? new List<string>() { },
            Label = FindLabelName?.Invoke(field),
            Description = FindLabelDescriptionName?.Invoke(field),
            Required = required
          };
        }
      });

      return builder;
    }

    protected virtual IRendererFieldBuilder Field(Expression<Func<T, object>> mapExpression, string label, string description = null, bool required = false, bool noDescription = false)
    {
      RendererFieldBuilder builder = new RendererFieldBuilder();

      Add(new RenderProperty()
      {
        Method = METHOD_FIELD,
        Compile = property =>
        {
          string field = mapExpression.CompileToJavascript().ToCamelCaseId();

          RendererFieldBuilder.Data fieldData = builder.Build();

          property.NestedRenderer = fieldData.Renderer;
          property.Params = new
          {
            Field = field,
            View = fieldData.View,
            ComponentPath = fieldData.ComponentPath,
            Options = fieldData.Options ?? new List<string>() { },
            Label = label != null && label.StartsWith("@") ? label : String.Format(LabelTemplate, label ?? field),
            Description = description != null && description.StartsWith("@") ? description : null, // TODO (noDescription ? null : String.Format(DescriptionTemplate, description ?? fieldTranslationName)),
            Required = required
          };
        }
      });

      return builder;
    }

    protected virtual void Tab(string name, Action builder)
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
    }

    protected virtual void Box(string name, string description, Action builder)
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
