namespace zero.Api.Endpoints.Media;

public class MediaBasic : BasicModel<zero.Media.Media>
{
  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}