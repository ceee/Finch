using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using System.Linq.Expressions;

namespace zero.Persistence;

public static class RavenQueryableExtensions
{
  /// <summary>
  /// 
  /// </summary>
  public static async Task<Paged<T>> FilterAsync<T>(this IRavenQueryable<T> source, ListQuery<T> listQuery)
  {
    return await source.WithFilter(listQuery).ToListResultAsync(listQuery);
  }


  /// <summary>
  /// 
  /// </summary>
  public static IQueryable<T> WithFilter<T>(this IRavenQueryable<T> source, ListQuery<T> listQuery)
  {
    Type collectionType = typeof(T);
    Type zeroType = typeof(ZeroEntity);
    bool isZeroType = zeroType.IsAssignableFrom(collectionType);

    source.Statistics(out QueryStatistics stats);
    listQuery.Statistics = stats;

    IQueryable<T> queryable = source;

    if (!listQuery.IncludeInactive && isZeroType)
    {
      queryable = queryable.Where(x => (x as ZeroEntity).IsActive);
    }

    if (!listQuery.Search.IsNullOrEmpty() && listQuery.SearchSelectors.Length > 0)
    {
      foreach (var selector in listQuery.SearchSelectors)
      {
        queryable = queryable.SearchIf(selector, listQuery.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.And);
      }
    }
    else if (!listQuery.Search.IsNullOrEmpty() && listQuery.SearchSelector != null)
    {
      queryable = queryable.SearchIf(listQuery.SearchSelector, listQuery.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.And);
    }

    if (listQuery.OrderQuery != null)
    {
      queryable = listQuery.OrderQuery(queryable);
    }
    else if (!listQuery.OrderBy.IsNullOrEmpty())
    {
      queryable = queryable.OrderBy(listQuery.OrderBy, listQuery.OrderIsDescending, listQuery.OrderType == ListQueryOrderType.String ? OrderingType.String : OrderingType.Double);
    }
    else if (isZeroType)
    {
      queryable = queryable.OrderByDescending(x => (x as ZeroEntity).CreatedDate);
    }

    if (listQuery.PageSize > 0)
    {
      queryable = queryable.Paging(listQuery.Page, listQuery.PageSize);
    }

    return queryable;
  }


  /// <summary>
  /// 
  /// </summary>
  public static async Task<Paged<T>> ToListResultAsync<T>(this IQueryable<T> source, ListQuery<T> listQuery, CancellationToken token = default)
  {
    List<T> results = await source.ToListAsync(token);
    return new Paged<T>(results, listQuery.Statistics.TotalResults, listQuery.Page, listQuery.PageSize);
  }


  public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string path, bool isDescending, OrderingType type = OrderingType.String)
  {
    if (!String.IsNullOrEmpty(path))
    {
      char[] a = path.ToCharArray();
      a[0] = char.ToUpper(a[0]);
      path = new string(a);
    }

    if (isDescending)
    {
      return source.OrderByDescending(path, type);
    }
    return source.OrderBy(path, type);
  }


  public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string path, bool isDescending, OrderingType type = OrderingType.String)
  {
    if (!String.IsNullOrEmpty(path))
    {
      char[] a = path.ToCharArray();
      a[0] = char.ToUpper(a[0]);
      path = new string(a);
    }

    if (isDescending)
    {
      return source.ThenByDescending(path, type);
    }
    return source.ThenBy(path, type);
  }


  public static IQueryable<T> Paging<T>(this IQueryable<T> source, int pageNumber, int pageSize)
  {
    pageNumber = pageNumber.Limit(1, 10_000_000);
    pageSize = pageSize.Limit(1, 1_000);

    if (pageNumber <= 0 || pageSize <= 0)
    {
      throw new NotSupportedException("Both pageNumber and pageSize must be greater than zero");
    }

    return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
  }


  public static IRavenQueryable<T> WhereIf<T>(this IRavenQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition, Expression<Func<T, bool>> elsePredicate = null)
  {
    if (!condition)
    {
      if (elsePredicate != null)
      {
        return source.Where(elsePredicate);
      }
      return source;
    }

    return source.Where(predicate);
  }


  public static IQueryable<T> SearchIf<T>(this IQueryable<T> source, Expression<Func<T, object>> fieldSelector, string searchTerms, string suffix = null, string prefix = null, SearchOperator @operator = SearchOperator.Or)
  {
    if (String.IsNullOrWhiteSpace(searchTerms))
    {
      return source;
    }

    searchTerms = searchTerms.Trim();

    string[] searchParts = searchTerms.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(x =>
    {
      if (suffix != null)
      {
        x += suffix;
      }
      if (prefix != null)
      {
        x = prefix + x;
      }
      return x;
    }).ToArray();

    if (searchTerms.StartsWith('"') && searchTerms.EndsWith('"'))
    {
      searchParts = new[] { searchTerms };
    }

    return source.Search(fieldSelector, searchParts, @operator: @operator);
  }
}
