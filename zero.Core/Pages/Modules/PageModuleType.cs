namespace zero.Pages;

/// <summary>
/// A module type holds information about a Module implementation
/// </summary>
public class PageModuleType : FlavorConfig
{
  /// <summary>
  /// Optionally group modules together
  /// </summary>
  public string Group { get; set; }

  /// <summary>
  /// Defining tags for a module type allows you to query for them
  /// </summary>
  public List<string> Tags { get; set; } = new();

  /// <summary>
  /// Page types where this module type is not allowed.
  /// This will only work when operating within the page context.
  /// </summary>
  public List<string> DisallowedPageTypes { get; set; } = new();


  public PageModuleType(Type type) : base(type) {}
}