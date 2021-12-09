namespace zero.Api.Endpoints.Media;

public class MediaSave : SaveModel<zero.Media.Media>
{
  public MediaType Type { get; set; }
}