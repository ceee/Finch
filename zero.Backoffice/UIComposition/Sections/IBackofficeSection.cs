namespace zero.Backoffice.UIComposition;

/// <summary>
/// Internal section
/// </summary>
internal interface IInternalBackofficeSection : IBackofficeSection { }

/// <summary>
/// A section is a main part of the backoffice application
/// </summary>
public interface IBackofficeSection
{
  /// <summary>
  /// The section alias which acts as the url slug for navigation
  /// </summary>
  string Alias { get; }

  /// <summary>
  /// The name of the section (either a string or a translation key with @ prefix)
  /// </summary>
  string Name { get; }

  /// <summary>
  /// Icon of the section
  /// </summary>
  string Icon { get; }

  /// <summary>
  /// HEX color (#aabbcc or #abc)
  /// </summary>
  string Color { get; }

  /// <summary>
  /// Children are displayed as a sub-navigation in the main nav area
  /// </summary>
  IList<IChildBackofficeSection> Children { get; }
}