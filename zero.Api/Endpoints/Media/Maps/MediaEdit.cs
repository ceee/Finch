namespace zero.Api.Endpoints.Media;

public class MediaEdit : DisplayModel<zero.Media.Media>
{
  public bool IsFolder { get; set; }

  public string ParentId { get; set; }
}