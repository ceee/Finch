using System;
using System.Collections.Generic;
using System.Linq;

namespace zero.Core.Entities
{
  /// <summary>
  /// Represents a paged result for a model collection
  /// </summary>
  public class ListResult<T>
  {
    public ListResult(long totalItems, long page, long pageSize)
    {
      TotalItems = totalItems;
      Page = page;
      PageSize = pageSize;

      if (pageSize > 0)
      {
        TotalPages = (long)Math.Ceiling(totalItems / (decimal)pageSize);
      }
      else
      {
        TotalPages = 1;
      }

      HasMore = TotalPages > Page;
    }

    public ListResult(IList<T> items, long totalItems, long pageNumber, long pageSize) : this(totalItems, pageNumber, pageSize)
    {
      Items = items;
    }

    public ListResult<TTarget> MapTo<TTarget>(Func<T, TTarget> convertItem)
    {
      return new ListResult<TTarget>(Items.Select(x => convertItem(x)).Where(x => x != null).ToList(), TotalItems, Page, PageSize)
      {
        Statistics = Statistics
      };
    }

    public long Page { get; private set; }

    public long PageSize { get; private set; }

    public long TotalPages { get; private set; }

    public long TotalItems { get; private set; }

    public bool HasMore { get; private set; }

    public IList<T> Items { get; set; }

    public List<PagedResultStatistic> Statistics { get; set; } = new List<PagedResultStatistic>();


    /// <summary>
    /// Calculates the skip size based on the paged parameters specified
    /// </summary>
    /// <remarks>
    /// Returns 0 if the page number or page size is zero
    /// </remarks>
    public int GetSkipSize()
    {
      if (Page > 0 && PageSize > 0)
      {
        return Convert.ToInt32((Page - 1) * PageSize);
      }
      return 0;
    }


    /// <summary>
    /// Adds a statistic value
    /// </summary>
    public ListResult<T> AddStatistic(string key, object value, StatisticDisplay display = StatisticDisplay.Default)
    {
      Statistics.Add(new PagedResultStatistic()
      {
        Key = key,
        Value = value,
        Display = display
      });
      return this;
    }
  }


  public class PagedResultStatistic
  {
    public string Key { get; set; }

    public object Value { get; set; }

    public StatisticDisplay Display { get; set; }
  }


  public enum StatisticDisplay
  {
    Default,
    Currency
  }


  public enum Direction
  {
    Ascending = 0,
    Descending = 1
  }
}
