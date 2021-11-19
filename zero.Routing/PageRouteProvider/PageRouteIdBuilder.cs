using zero.Core.Entities;

namespace zero.Routing;

public class PageRouteIdBuilder : IPageRouteIdBuilder
{
  /// <inheritdoc />
  public string Generate(Page page)
  {
    return "routes." + Constants.Pages.PageRouteProviderAlias + "." + page.Hash;
  }
}


public interface IPageRouteIdBuilder
{
  /// <summary>
  /// Generate ID for a page
  /// </summary>
  string Generate(Page page);
}
