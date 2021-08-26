using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Extensions
{
  public static class EnumerableExtensions
  {
    /// <summary>
    /// 
    /// </summary>
    public static ListResult<T> ToQueriedList<T>(this IEnumerable<T> items, ListQuery<T> query) where T : ZeroEntity
    {
      //queryable = queryable.Statistics(out QueryStatistics stats);

      if (query != null)
      {
        if (!query.IncludeInactive)
        {
          items = items.Where(x => x.IsActive);
        }

        if (!query.Search.IsNullOrEmpty())
        {
          items = items.Where(x => x.Name.Contains(query.Search, StringComparison.InvariantCultureIgnoreCase));
        }
        //if (!query.Search.IsNullOrEmpty() && query.SearchSelector != null)
        //{
        //  items = items.SearchIf(query.SearchSelector, query.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.And);
        //}
        //if (!query.Search.IsNullOrEmpty() && query.SearchSelectors.Length > 0)
        //{
        //  foreach (var selector in query.SearchSelectors)
        //  {
        //    items = items.SearchIf(selector, query.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.And);
        //  }
        //}

        //if (!query.OrderBy.IsNullOrEmpty())
        //{
        //  items = items.OrderBy(query.OrderBy, query.OrderIsDescending);
        //}

        if (query.PageSize > 0)
        {
          items = items.Paging(query.Page, query.PageSize);
        }
      }

      List<T> result = items.ToList();

      return new ListResult<T>(result, result.Count, query.Page, query.PageSize);
    }



    public static IEnumerable<T> Paging<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
      pageNumber = pageNumber.Limit(1, 10_000_000);
      pageSize = pageSize.Limit(1, 1_000);

      if (pageNumber <= 0 || pageSize <= 0)
      {
        throw new NotSupportedException("Both pageNumber and pageSize must be greater than zero");
      }

      return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }


    public static bool TryGet<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T model)
    {
      model = source.FirstOrDefault(predicate);
      return model != null;
    }
  }
}
