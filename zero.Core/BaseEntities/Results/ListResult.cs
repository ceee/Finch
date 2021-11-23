namespace zero;

/// <summary>
/// Represents a paged result for a model collection
/// </summary>
public class ListResult<T> : ListResult
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
    return new ListResult<TTarget>(Items.Select(x => convertItem(x)).Where(x => x != null).ToList(), TotalItems, Page, PageSize);
  }

  public IList<T> Items { get; set; } = new List<T>();


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
}


public class ListResult
{
  public long Page { get; protected set; }

  public long PageSize { get; protected set; }

  public long TotalPages { get; protected set; }

  public long TotalItems { get; protected set; }

  public bool HasMore { get; protected set; }
}