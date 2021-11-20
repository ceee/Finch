using FluentValidation.Internal;
using System.Linq.Expressions;
using System.Reflection;

namespace zero.Validation;

public class ValidatorCamelCasePropertyResolver
{
  public static string ResolvePropertyName(Type type, MemberInfo memberInfo, LambdaExpression expression)
  {
    return DefaultPropertyNameResolver(type, memberInfo, expression).ToCamelCaseId();
  }

  private static string DefaultPropertyNameResolver(Type type, MemberInfo memberInfo, LambdaExpression expression)
  {
    if (expression != null)
    {
      var chain = PropertyChain.FromExpression(expression);
      if (chain.Count > 0) return chain.ToString();
    }

    if (memberInfo != null)
    {
      return memberInfo.Name;
    }

    return null;
  }
}