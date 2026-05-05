namespace Mixtape.Identity;

public class UserToken
{
  /// <summary>
  /// Gets or sets the provider for this instance.
  /// </summary>
  public string LoginProvider { get; set; }

  /// <summary>
  /// The token.
  /// </summary>
  public string Value { get; set; }

  /// <summary>
  /// Gets or sets the name of the token.
  /// </summary>
  public string Name { get; set; }
}