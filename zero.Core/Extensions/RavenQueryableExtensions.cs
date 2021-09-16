using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Extensions
{
  public static class RavenQueryableExtensions
  {
    // TODO we need to simplify these extensions methods.
    // ToQueriedListAsyncX is used in MediaCollection for the Media_ByParent index, which produces MediaListItem (which is no ZeroEntity)
    /// <summary>
    /// 
    /// </summary>
    public static async Task<ListResult<T>> ToQueriedListAsyncX<T>(this IRavenQueryable<T> queryable, ListQuery<T> query)
    {
      queryable = queryable.Statistics(out QueryStatistics stats);

      IQueryable<T> rawQuery = queryable;

      if (query != null)
      {
        if (!query.Search.IsNullOrEmpty() && query.SearchSelector != null)
        {
          rawQuery = rawQuery.SearchIf(query.SearchSelector, query.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.And);
        }
        if (!query.Search.IsNullOrEmpty() && query.SearchSelectors.Length > 0)
        {
          foreach (var selector in query.SearchSelectors)
          {
            rawQuery = rawQuery.SearchIf(selector, query.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.And);
          }
        }

        if (query.OrderQuery != null)
        {
          rawQuery = query.OrderQuery(rawQuery);
        }
        else if (!query.OrderBy.IsNullOrEmpty())
        {
          rawQuery = rawQuery.OrderBy(query.OrderBy, query.OrderIsDescending, query.OrderType == ListQueryOrderType.String ? OrderingType.String : OrderingType.Double);
        }

        if (query.PageSize > 0)
        {
          rawQuery = rawQuery.Paging(query.Page, query.PageSize);
        }
      }

      List<T> items = await rawQuery.ToListAsync();

      return new ListResult<T>(items, stats.TotalResults, query.Page, query.PageSize);
    }


    /// <summary>
    /// 
    /// </summary>
    public static async Task<ListResult<T>> ToQueriedListAsync<T>(this IRavenQueryable<T> queryable, ListQuery<T> query) where T : ZeroEntity
    {
      queryable = queryable.Statistics(out QueryStatistics stats);

      IQueryable<T> rawQuery = queryable;

      if (query != null)
      {
        if (!query.IncludeInactive)
        {
          rawQuery = rawQuery.Where(x => x.IsActive);
        }

        if (!query.Search.IsNullOrEmpty() && query.SearchSelector != null)
        {
          rawQuery = rawQuery.SearchIf(query.SearchSelector, query.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.And);
        }
        if (!query.Search.IsNullOrEmpty() && query.SearchSelectors.Length > 0)
        {
          foreach (var selector in query.SearchSelectors)
          {
            rawQuery = rawQuery.SearchIf(selector, query.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.And);
          }
        }

        if (query.OrderQuery != null)
        {
          rawQuery = query.OrderQuery(rawQuery);
        }
        else if (!query.OrderBy.IsNullOrEmpty())
        {
          rawQuery = rawQuery.OrderBy(query.OrderBy, query.OrderIsDescending, query.OrderType == ListQueryOrderType.String ? OrderingType.String : OrderingType.Double);
        }
        else
        {
          rawQuery = rawQuery.OrderByDescending(x => x.CreatedDate);
        }

        if (query.PageSize > 0)
        {
          rawQuery = rawQuery.Paging(query.Page, query.PageSize);
        }
      }

      List<T> items = await rawQuery.ToListAsync();

      return new ListResult<T>(items, stats.TotalResults, query.Page, query.PageSize);
    }


    /// <summary>
    /// 
    /// </summary>
    public static async Task<ListResult<T>> ToQueriedListAsyncX<T, TFilter>(this IRavenQueryable<T> queryable, ListQuery<T, TFilter> query) where TFilter : IListSpecificQuery
    {
      queryable = queryable.Statistics(out QueryStatistics stats);

      IQueryable<T> rawQuery = queryable;

      if (query != null)
      {
        if (!query.Search.IsNullOrEmpty() && query.SearchSelector != null)
        {
          rawQuery = rawQuery.SearchIf(query.SearchSelector, query.Search, "*", "*");
        }
        if (!query.Search.IsNullOrEmpty() && query.SearchSelectors.Length > 0)
        {
          foreach (var selector in query.SearchSelectors)
          {
            rawQuery = rawQuery.SearchIf(selector, query.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.Or);
          }
        }

        if (!query.OrderBy.IsNullOrEmpty())
        {
          rawQuery = rawQuery.OrderBy(query.OrderBy, query.OrderIsDescending, query.OrderType == ListQueryOrderType.String ? OrderingType.String : OrderingType.Double);
        }

        if (query.PageSize > 0)
        {
          rawQuery = rawQuery.Paging(query.Page, query.PageSize);
        }
      }

      List<T> items = await rawQuery.ToListAsync();

      return new ListResult<T>(items, stats.TotalResults, query.Page, query.PageSize);
    }


    /// <summary>
    /// 
    /// </summary>
    public static async Task<ListResult<T>> ToQueriedListAsync<T, TFilter>(this IRavenQueryable<T> queryable, ListQuery<T, TFilter> query) where T : ZeroEntity where TFilter : IListSpecificQuery
    {
      queryable = queryable.Statistics(out QueryStatistics stats);

      IQueryable<T> rawQuery = queryable;

      if (query != null)
      {
        if (!query.IncludeInactive)
        {
          rawQuery = rawQuery.Where(x => x.IsActive);
        }

        if (!query.Search.IsNullOrEmpty() && query.SearchSelector != null)
        {
          rawQuery = rawQuery.SearchIf(query.SearchSelector, query.Search, "*", "*");
        }
        if (!query.Search.IsNullOrEmpty() && query.SearchSelectors.Length > 0)
        {
          foreach (var selector in query.SearchSelectors)
          {
            rawQuery = rawQuery.SearchIf(selector, query.Search, "*", "*", Raven.Client.Documents.Queries.SearchOperator.Or);
          }
        }

        if (!query.OrderBy.IsNullOrEmpty())
        {
          rawQuery = rawQuery.OrderBy(query.OrderBy, query.OrderIsDescending, query.OrderType == ListQueryOrderType.String ? OrderingType.String : OrderingType.Double);
        }

        if (query.PageSize > 0)
        {
          rawQuery = rawQuery.Paging(query.Page, query.PageSize);
        }
      }

      List<T> items = await rawQuery.ToListAsync();

      return new ListResult<T>(items, stats.TotalResults, query.Page, query.PageSize);
    }
  }
}
