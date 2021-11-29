namespace zero.Api.Models;

public enum ListQueryDisplayType
{
  /// <summary>
  /// Default output for listings
  /// </summary>
  Default = 0,
  /// <summary>
  /// Previews for collection pickers
  /// </summary>
  Preview = 1,
  /// <summary>
  /// Selection within a picker
  /// </summary>
  Picker = 2
}