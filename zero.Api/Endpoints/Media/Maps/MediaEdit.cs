namespace zero.Api.Endpoints.Media;

public class MediaEdit : DisplayModel<zero.Media.Media>
{
  public MediaType Type { get; set; }

  public string ParentId { get; set; }
}