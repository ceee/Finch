using System.Collections.Generic;

namespace zero.Core.Entities
{
  public interface IRoutedEntity : IZeroEntity
  {
    /// <summary>
    /// Url for this entity
    /// </summary>
    public UrlRoute Route { get; set; }
  }


  public class UrlRoute
  {
    /// <summary>
    /// Url for this entity
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Route dependencies can be used for cache busting
    /// </summary>
    public IList<string> Dependencies { get; set; } = new List<string>();
  }
}
