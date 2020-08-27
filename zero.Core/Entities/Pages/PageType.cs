using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
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

    /// <summary>
    /// Whether this page type can be used as a website entry point
    /// </summary>
    public bool AllowAsRoot { get; set; }

    /// <summary>
    /// This page type is only allowed at the root level (ignoring other properties)
    /// </summary>
    public bool OnlyAtRoot { get; set; }

    /// <summary>
    /// Whether all page types can be created as children of this type
    /// </summary>
    public bool AllowAllChildrenTypes { get; set; }

    /// <summary>
    /// Page types which are allowed as children
    /// </summary>
    public List<string> AllowedChildrenTypes { get; set; } = new List<string>();


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
        Icon = model.Icon,
        AllowAllChildrenTypes = model.AllowAllChildrenTypes,
        AllowedChildrenTypes = model.AllowedChildrenTypes,
        AllowAsRoot = model.AllowAsRoot,
        OnlyAtRoot = model.OnlyAtRoot
      };
    }
  }
}
