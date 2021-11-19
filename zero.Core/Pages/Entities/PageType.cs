using System;

namespace zero;

/// <summary>
/// A page type holds information about a Page implementation
/// </summary>
public class PageType<T> : PageType where T : Page, new()
{
  public PageType() : base(typeof(T)) { }
}


/// <summary>
/// A page type holds information about a Page implementation
/// </summary>
public class PageType
{
  /// <summary>
  /// Type of the associated page class
  /// </summary>
  public Type ContentType { get; private set; }

  /// <summary>
  /// Alias for querying
  /// </summary>
  public string Alias { get; set; }

  /// <summary>
  /// Name of the page type
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Optional description
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// Icon of the page type
  /// </summary>
  public string Icon { get; set; }


  public PageType(Type type)
  {
    ContentType = type;
  }

  public static PageType Convert<T>(PageType<T> model) where T : Page, new()
  {
    return new PageType(model.ContentType)
    {
      Alias = model.Alias,
      Name = model.Name,
      Description = model.Description,
      Icon = model.Icon
    };
  }
}
