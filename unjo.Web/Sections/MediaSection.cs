using System.Collections.Generic;
using unjo.Core;
using unjo.Core.Entities;

namespace unjo.Web.Sections
{
  /// <summary>
  /// Media items (images, videos, documents) grouped in folders
  /// </summary>
  public class MediaSection : ISection
  {
    /// <inheritdoc />
    public string Alias => Constants.Sections.Media;

    /// <inheritdoc />
    public string Name => "@ui_sections_media";

    /// <inheritdoc />
    public string Icon => "fth-image";

    /// <inheritdoc />
    public IList<IChildSection> Children => null;
  }
}
