using System.Collections.Generic;

namespace zero.Core.Entities
{
  /// <summary>
  /// A section is a main part of the backoffice application
  /// </summary>
  public class Section : ISection, IChildSection
  {
    /// <inheritdoc />
    public string Alias { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string Icon { get; }

    /// <summary>
    /// HEX color (#aabbcc or #abc). Defaults to a neutral color
    /// </summary>
    public string Color { get; }

    /// <inheritdoc />
    public IList<IChildSection> Children { get; } = new List<IChildSection>();


    public Section() { }

    public Section(string alias, string name, string icon = null, string color = null)
    {
      Alias = alias;
      Name = name;
      Icon = icon;
      Color = color;
    }
  }
}
