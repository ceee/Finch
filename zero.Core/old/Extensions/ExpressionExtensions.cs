using System.Linq.Expressions;
using System.Text;

namespace zero.Core.Extensions
{
  public static class ExpressionExtensions
  {
    internal static string GetPropertyPath(this Expression expr, out MemberExpression member)
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

    internal static MemberExpression GetMemberExpression(this Expression expression)
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
