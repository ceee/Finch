using System.Collections.Generic;

namespace zero.Core.Entities
{
  /// <summary>
  /// A section is a main part of the backoffice application
  /// </summary>
  public class Section : ISection
  {
    /// <inheritdoc />
    public string Alias { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string Icon { get; }

    /// <inheritdoc />
    public IList<IChildSection> Children { get; } = new List<IChildSection>();


    public Section() { }

    public Section(string alias, string name, string icon)
    {
      Alias = alias;
      Name = name;
      Icon = icon;
    }
  }
}
