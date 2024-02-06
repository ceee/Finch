namespace zero.Metadata;

public class MetadataOptions
{
  public List<string> TitleFragments { get; set; } = [];

  public bool? NoIndex { get; set; }

  public bool? NoFollow { get; set; }

  public string Description { get; set; }

  public string Icon { get; set; }

  public string Image { get; set; }

  public string Author { get; set; }

  //public Schema Schema { get; set; }

  public MetadataOptions() { }

  public MetadataOptions(params string[] titleFragments)
  {
    TitleFragments.AddRange(titleFragments);
  }
}
