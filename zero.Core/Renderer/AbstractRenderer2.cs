using Lambda2Js;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using zero.Core.Extensions;

namespace zero.Core.Renderer
{
  public abstract class AbstractRenderer2<T>
  {
    const string METHOD_FIELD = "field";

    const string METHOD_TAB = "tab";

    const string METHOD_BOX = "box";

    protected List<RenderProperty> Properties { get; set; } = new List<RenderProperty>();

    RenderProperty ParentProperty = null;

    int CurrentDepth = 0;



    /// <summary>
    /// Create a field
    /// </summary>
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
            //Label = FindLabelName?.Invoke(field),
            //Description = FindLabelDescriptionName?.Invoke(field),
            Required = required
          };
        }
      });

      return builder;
    }


    /// <summary>
    /// Creates a tab
    /// </summary>
    protected virtual void Tab(string name, Action contentBuilder)
    {
      Add(new RenderProperty()
      {
        Method = METHOD_TAB,
        Builder = contentBuilder,
        Params = new
        {
          Name = name
        }
      });
    }


    /// <summary>
    /// Creates a visual box
    /// </summary>
    protected virtual void Box(string name, string description, Action contentBuilder)
    {
      Add(new RenderProperty()
      {
        Method = METHOD_BOX,
        Builder = contentBuilder,
        Params = new
        {
          Name = name,
          Description = description
        }
      });
    }


    /// <summary>
    /// Adds a property to the list
    /// </summary>
    void Add(RenderProperty property)
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
