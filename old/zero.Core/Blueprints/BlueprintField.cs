using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Blueprints
{
  public class BlueprintField<T> where T : ZeroEntity
  {
    public Expression<Func<T, object>> Expression { get; private set; }

    public PropertyInfo PropertyInfo { get; private set; }

    public string FieldName { get; private set; }

    Action<T, T> _applyHook { get; set; }

    Func<T, object> _selectorFunc = null;


    internal BlueprintField(Expression<Func<T, object>> selector)
    {
      Expression = selector;
      FieldName = selector.GetPropertyPath(out MemberExpression member);
      PropertyInfo = member?.Member as PropertyInfo;

      if (PropertyInfo == null)
      {
        throw new ArgumentException("selector parameter has to be a property selector");
      }

      if (FieldName.IsNullOrEmpty())
      {
        throw new Exception("Could not find a matching field name");
      }
    }


    public virtual void Apply(T blueprint, T model)
    {
      if (_applyHook != null)
      {
        _applyHook(blueprint, model);
        return;
      }

      _selectorFunc ??= Expression.Compile();
      PropertyInfo.SetValue(model, _selectorFunc(blueprint));
    }


    public virtual void OnUpdate(Action<T, T> onUpdate)
    {
      _applyHook = onUpdate;
    }
  }
}
