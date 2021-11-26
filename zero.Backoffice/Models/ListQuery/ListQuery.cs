using System.Linq.Expressions;

namespace zero.Backoffice.Models;

public class ListQuery<T>
{
  public ListQueryDisplayType DisplayType { get; set; }

  public string[] Ids { get; set; } = Array.Empty<string>();

  public string Search { get; set; } = null;

  public Expression<Func<T, object>> SearchSelector { get; set; } = null;

  public Expression<Func<T, object>>[] SearchSelectors { get; private set; } = new Expression<Func<T, object>>[0] { };

  public string OrderBy { get; set; } = "createdDate";

  public ListQueryOrderType OrderType { get; set; } = ListQueryOrderType.String;

  public bool OrderIsDescending { get; set; } = true;

  public Func<IQueryable<T>, IQueryable<T>> OrderQuery = null;

  public int Page { get; set; } = 1;

  public int PageSize { get; set; } = 30;

  //public bool IncludeInactive { get; set; } = true;

  public void SearchFor(params Expression<Func<T, object>>[] selectors)
  {
    SearchSelectors = selectors;
  }
}


public class ListQuery<T, TFilter> : ListQuery<T> where TFilter : IListSpecificQuery
{
  public TFilter Filter { get; set; }
}