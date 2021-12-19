namespace zero.Api.Endpoints.Pages;

public class PageBasic : ZeroIdEntity
{
  public string ParentId { get; set; }

  public string Name { get; set; }

  public int Children { get; set; }
}