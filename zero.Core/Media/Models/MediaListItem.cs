namespace zero.Media;

public class MediaListItem : ZeroIdEntity
{
  public string ParentId { get; set; }

  public string Name { get; set; }

  public DateTimeOffset CreatedDate { get; set; }

  public bool IsFolder { get; set; }

  public string Image { get; set; }

  public long Size { get; set; }

  public int Children { get; set; }

  public bool HasTransparency { get; set; }

  public float AspectRatio { get; set; }

  public bool IsShared { get; set; }
}
