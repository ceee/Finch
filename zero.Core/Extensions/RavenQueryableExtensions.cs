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
    public static IRavenQueryable<T> ForApp<T>(this IRavenQueryable<T> source, string appId, bool includeShared = false) where T : IDatabaseEntity
    {
      if (appId.IsNullOrEmpty())
      {
        return source;
      }

      HashSet<string> ids = new HashSet<string>();
      ids.Add(appId);

      if (includeShared)
      {
        ids.Add(Constants.Database.SharedAppId);
      }

      return source.Where(item => item.AppId.In(ids));
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
