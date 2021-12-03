namespace zero.Api.Endpoints.Pages;

public class PageEdit : ZeroIdEntity
{
  // we can't use Page as property type here
  // cause that would serialize flavors to Page and remove additional properties.
  // according to the System.Text.Json docs using "object" will serialize to the implementing type
  public object Page { get; set; }

  public FlavorConfig PageType { get; set; }

  public List<string> Urls { get; set; } = new();
}