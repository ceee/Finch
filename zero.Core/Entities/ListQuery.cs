using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace zero.Core.Entities
{
  [BindProperties(SupportsGet = true)]
  public class ListQuery<T> : ListQuery<T, EmptyListSpecificQuery> { }

  [BindProperties(SupportsGet = true)]
  public class ListQuery<T, TFilter> where TFilter : IListSpecificQuery
  {
    public string Search { get; set; } = null;

    public Expression<Func<T, object>> SearchSelector { get; set; } = null;

    public Expression<Func<T, object>>[] SearchSelectors { get; private set; } = new Expression<Func<T, object>>[0] { };

    public string OrderBy { get; set; } = "createdDate";

    public ListQueryOrderType OrderType { get; set; } = ListQueryOrderType.String;

    public bool OrderIsDescending { get; set; } = true;

    public Func<IQueryable<T>, IQueryable<T>> OrderQuery = null;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 30;

    public TFilter Filter { get; set; }

    public void SearchFor(params Expression<Func<T, object>>[] selectors)
    {
      SearchSelectors = selectors;
    }
  }


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
  }


  public interface IListSpecificQuery { }

  public class EmptyListSpecificQuery : IListSpecificQuery { }


  public class ListQueryDateRange
  {
    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }
  }

  public class ListQueryRange
  {
    public decimal? From { get; set; }

    public decimal? To { get; set; }
  }

  public enum ListQueryOrderType
  {
    String,
    Number
  }
}
