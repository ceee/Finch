namespace zero.Api.Endpoints.Media;

public class MediaFileEdit : MediaEdit
{
  public string AlternativeText { get; set; }

  public string Caption { get; set; }

  public string Path { get; set; }

  public Dictionary<string, string> Thumbnails { get; set; } = new();

  public long Size { get; set; }

  public MediaImageMetadata ImageMeta { get; set; }

  public MediaFocalPoint FocalPoint { get; set; }
}