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

    /// <summary>
    /// This field is synced as long as it's not desynced for a certain entity
    /// </summary>
    public bool IsSynced { get; set; }

    /// <summary>
    /// This field is synced and cannot be desynced
    /// </summary>
    public bool IsLocked { get; set; }

    Action<T, T> _applyHook { get; set; }

    Func<T, object> _selectorFunc = null;


    internal BlueprintField(Expression<Func<T, object>> selector)
    {
      Expression = selector;
      FieldName = GetPropertyPath(selector, out MemberExpression member);
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


    static string GetPropertyPath(Expression expr, out MemberExpression member)
    {
      StringBuilder path = new();
      MemberExpression memberExpression = GetMemberExpression(expr);
      do
      {
        member = memberExpression;

        if (path.Length > 0)
        {
          path.Insert(0, ".");
        }
        path.Insert(0, memberExpression.Member.Name);
        memberExpression = GetMemberExpression(memberExpression.Expression);
      }
      while (memberExpression != null);

      return path.ToString();
    }


    static MemberExpression GetMemberExpression(Expression expression)
    {
      if (expression is MemberExpression)
      {
        return (MemberExpression)expression;
      }
      else if (expression is LambdaExpression)
      {
        var lambdaExpression = expression as LambdaExpression;
        if (lambdaExpression.Body is MemberExpression)
        {
          return (MemberExpression)lambdaExpression.Body;
        }
        else if (lambdaExpression.Body is UnaryExpression)
        {
          return ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
        }
      }
      return null;
    }
  }
}
