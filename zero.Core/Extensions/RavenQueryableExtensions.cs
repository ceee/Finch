using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Extensions
{
  public static class RavenQueryableExtensions
  {
    static Type _appAwareEntity = typeof(IAppAwareEntity);

    public static IRavenQueryable<T> Scope<T>(this IRavenQueryable<T> source, string appId, bool includeShared = true)
    {
      if (appId.IsNullOrEmpty() || !_appAwareEntity.IsAssignableFrom(source.ElementType))
      {
        return source;
      }

      HashSet<string> ids = new HashSet<string>();
      ids.Add(appId);

      if (includeShared)
      {
        ids.Add(Constants.Database.SharedAppId);
      }

      return source.Where(item => (item as IAppAwareEntity).AppId.In(ids));
    }


    public static IRavenQueryable<T> Scope<T>(this IRavenQueryable<T> source, ApiScope scope)
    {
      if (scope == null || scope.Global)
      {
        return source;
      }

      if (scope.AppId.IsNullOrEmpty() || !_appAwareEntity.IsAssignableFrom(source.ElementType))
      {
        return source;
      }

      HashSet<string> ids = new HashSet<string>();
      ids.Add(scope.AppId);

      if (scope.IncludeShared)
      {
        ids.Add(Constants.Database.SharedAppId);
      }

      return source.Where(item => (item as IAppAwareEntity).AppId.In(ids));
    }


    /// <summary>
    /// 
    /// </summary>
    public static async Task<ListResult<T>> ToQueriedListAsync<T>(this IRavenQueryable<T> queryable, ListQuery<T> query)
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
    public static async Task<ListResult<T>> ToQueriedListAsync<T, TFilter>(this IRavenQueryable<T> queryable, ListQuery<T, TFilter> query) where TFilter : IListSpecificQuery
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
  }
}
