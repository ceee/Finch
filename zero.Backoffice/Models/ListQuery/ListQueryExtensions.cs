using Newtonsoft.Json;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace zero.Backoffice.Models;

public static class ListQueryExtensions
{
  public static ListQuery<T, TFilter> AsList<T, TFilter>(this string options) where TFilter : IListSpecificQuery
  {
    return JsonConvert.DeserializeObject<ListQuery<T, TFilter>>(options);
  }

  public static ListQuery<T> AsList<T>(this string options) where T : IListSpecificQuery
  {
    return JsonConvert.DeserializeObject<ListQuery<T>>(options);
  }

  public static IQueryable<T> Filter<T>(this IRavenQueryable<T> source, ListQuery<T> listQuery) where T : ZeroIdEntity
  {
    if (listQuery == null)
    {
      return source;
    }

    Type collectionType = typeof(T);
    Type zeroType = typeof(ZeroEntity);
    bool isZeroType = zeroType.IsAssignableFrom(collectionType);

    IQueryable<T> queryable = source;
    
    if (listQuery.Ids.Length > 0)
    {
      queryable = queryable.Where(x => x.Id.In(listQuery.Ids));
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

    return queryable;
  }


  public static Paged<ZeroIdEntity> Convert<T>(this Paged<T> result, ListQueryDisplayType displayType) where T : ZeroIdEntity
  {

  }
}