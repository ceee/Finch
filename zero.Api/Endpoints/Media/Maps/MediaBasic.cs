namespace zero.Api.Endpoints.Media;

public class MediaBasic : ZeroIdEntity
{
  public string ParentId { get; set; }

  public string Name { get; set; }

  public bool IsFolder { get; set; }

  public string Source { get; set; }

  public string Preview { get; set; }

  public int Children { get; set; }
  
  public long Size { get; set; }
}