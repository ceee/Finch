namespace zero.Rendering;

public class IconOptions : IconSetOptions
{
  public IconOptions()
  {
    CssClass = "app-icon";
  }

  public List<IconSetOptions> Sets { get; set; } = [];
}


public class IconSetOptions
{
  public string Key { get; set; }

  public string Path { get; set; }

  public string CssClass { get; set; }

  public int? DefaultSize { get; set; } = 18;

  public decimal? DefaultStrokeWidth { get; set; } = 2;
}