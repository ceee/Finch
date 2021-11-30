namespace zero.Models;

public class Paged<T> : Paged
{
  public IList<T> Items { get; set; } = new List<T>();

  public Paged(IList<T> items, long totalItems, long pageNumber, long pageSize) : base(totalItems, pageNumber, pageSize)
  {
    Items = items;
  }

  public Paged<TTarget> MapTo<TTarget>(Func<T, TTarget> convertItem)
  {
    return new Paged<TTarget>(Items.Select(x => convertItem(x)).Where(x => x != null).ToList(), TotalItems, Page, PageSize);
  }

  public override IEnumerable GetItems()
  {
    return Items;
  }
}


public abstract class Paged
{
  public long Page { get; protected set; }

  public long PageSize { get; protected set; }

  public long TotalPages { get; protected set; }

  public long TotalItems { get; protected set; }

  public bool HasMore { get; protected set; }


  public Paged(long totalItems, long page, long pageSize)
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

  public int GetSkipSize()
  {
    if (Page > 0 && PageSize > 0)
    {
      return Convert.ToInt32((Page - 1) * PageSize);
    }
    return 0;
  }

  public abstract IEnumerable GetItems();
}