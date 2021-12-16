namespace zero.Api.Endpoints.Media;

public class MediaSave : SaveModel<zero.Media.Media>
{
  public bool IsFolder { get; set; }
}