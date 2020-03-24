namespace zero.Core.Entities
{
  /// <summary>
  /// A child section is a sub-navigation item of a section
  /// </summary>
  public interface IChildSection
  {
    /// <summary>
    /// The section alias which acts as the url slug for navigation
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// The name of the section (either a string or a translation key with @ prefix)
    /// </summary>
    public string Name { get; set; }
  }
}
