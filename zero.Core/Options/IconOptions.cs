using zero.Core.Entities;

namespace zero.Core.Options
{
  public class IconOptions : ZeroBackofficeCollection<IconSet>, IZeroCollectionOptions
  {
    /// <summary>
    /// Add a new backoffice icon set
    /// </summary>
    /// <param name="alias">Alias for reference</param>
    /// <param name="name">Name of the icon set</param>
    /// <param name="spritePath">Path to the SVG sprite containing addressable symbols</param>
    /// <param name="prefix">Optional symbol identifier prefix (by default the alias is used)</param>
    public void AddSet(string alias, string name, string spritePath, string prefix = null)
    {
      Items.Add(new IconSet()
      {
        Alias = alias,
        Name = name,
        SpritePath = spritePath,
        Prefix = prefix ?? alias
      });
    }
  }
}
