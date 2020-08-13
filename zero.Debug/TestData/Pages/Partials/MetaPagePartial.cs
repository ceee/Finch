using zero.Core.Entities;

namespace zero.TestData
{
  public class MetaPagePartial
  {
    public bool HideInTitle { get; set; }

    public string TitleOverride { get; set; }

    public string TitleOverrideAll { get; set; }

    public string SeoDescription { get; set; }

    public string SeoImageId { get; set; }

    public bool NoFollow { get; set; }

    public bool NoIndex { get; set; }
  }
}
