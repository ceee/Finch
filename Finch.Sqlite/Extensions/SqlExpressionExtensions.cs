using System;
using System.Linq.Expressions;
using ServiceStack.OrmLite;
using Finch.Extensions;

namespace Finch.Sqlite;

public static class SqlExpressionExtensions
{
  public static SqlExpression<T> Paging<T>(this SqlExpression<T> source, int pageNumber, int pageSize)
  {
    pageNumber = pageNumber.Limit(1, 10_000_000);
    pageSize = pageSize.Limit(1, 1_000);

    if (pageNumber <= 0 || pageSize <= 0)
    {
      throw new NotSupportedException("Both pageNumber and pageSize must be greater than z_ero");
    }

    return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
  }

  public static SqlExpression<T> WhereIf<T>(this SqlExpression<T> source, Expression<Func<T, bool>> predicate, bool condition, Expression<Func<T, bool>> elsePredicate = null)
  {
    if (condition)
    {
      return source.Where(predicate);
    }

    return elsePredicate != null ? source.Where(elsePredicate) : source;
  }
}
