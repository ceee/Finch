using FluentValidation;
using System;
using System.Linq.Expressions;

namespace zero.Core.Renderer
{
  public abstract class AbstractRenderer<T> : IRenderer<T>
  {
    protected string LabelTemplate = null;

    protected string DescriptionTemplate = null;

    protected IValidator<T> Validator = null;

    protected virtual IRendererFieldBuilder Field(Expression<Func<T, object>> mapExpression, string label = null, string description = null, bool required = false)
    {
      return null;
    }

    protected virtual IRenderer<T> Tab(string name, Action builder)
    {
      return null;
    }

    protected virtual IRenderer<T> Box(string name, string description, Action builder)
    {
      return null;
    }
  }
}
