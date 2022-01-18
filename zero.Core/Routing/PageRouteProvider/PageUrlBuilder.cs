using System.Text;

namespace zero.Routing;

public class PageUrlBuilder : IPageUrlBuilder
{
  const char PATH_SEPARATOR = '/';

  const bool TRAILING_SLASH = false;

  protected IZeroStore Store { get; private set; }

  protected PageOptions Options { get; private set; }


  public PageUrlBuilder(IZeroStore store, IZeroOptions options)
  {
    Store = store;
    Options = options.For<PageOptions>();
  }


  /// <inheritdoc />
  public string GetUrl(Page page, IEnumerable<Page> parents)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(PATH_SEPARATOR);

    void AddPart(Page page)
    {
      string part = GetUrlPart(page);

      if (!part.IsNullOrEmpty())
      {
        stringBuilder.Append(part);
        stringBuilder.Append(PATH_SEPARATOR);
      }
    }

    if (parents != null)
    {
      foreach (Page parent in parents)
      {
        AddPart(parent);
      }
    }

    AddPart(page);

    if (!TRAILING_SLASH && stringBuilder.Length > 1)
    {
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
    }

    return stringBuilder.ToString();
  }


  /// <inheritdoc />
  public virtual string GetUrlPart(Page page)
  {
    string alias;

    if (page == null)
    {
      return null;
    }

    if (page is PageFolder && !((PageFolder)page).IsPartOfRoute)
    {
      return null;
    }

    if (!page.UrlAlias.IsNullOrWhiteSpace())
    {
      alias = page.UrlAlias;
    }
    else if (page.Flavor == Options.Root)
    {
      return String.Empty;
    }
    else if (!page.Alias.IsNullOrWhiteSpace())
    {
      alias = page.Alias;
    }
    else
    {
      alias = page.Name;
    }

    return Safenames.Alias(alias).Trim().Trim(PATH_SEPARATOR);
  }
}


public interface IPageUrlBuilder
{
  /// <summary>
  /// Get URL for a page
  /// </summary>
  string GetUrl(Page page, IEnumerable<Page> parents);

  /// <summary>
  /// Get the part of the URL (by querying UrlAlias and Alias) for this page
  /// </summary>
  string GetUrlPart(Page page);
}
