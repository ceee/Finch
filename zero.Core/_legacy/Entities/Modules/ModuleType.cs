using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  /// <summary>
  /// A module type holds information about a Module implementation
  /// </summary>
  public class ModuleType<T> : ModuleType where T : Module, new()
  {
    public ModuleType() : base(typeof(T)) { }
  }


  /// <summary>
  /// A module type holds information about a Module implementation
  /// </summary>
  public class ModuleType
  {
    /// <summary>
    /// Type of the associated module class
    /// </summary>
    public Type ContentType { get; private set; }

    /// <summary>
    /// Alias for querying
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// Name of the module type
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Optional description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Icon of the module type
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// Optionally group modules together
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// Defining tags for a module type allows you to query for them
    /// </summary>
    public List<string> Tags { get; set; } = new List<string>();

    /// <summary>
    /// Page types where this module type is not allowed.
    /// This will only work when operating within the page context.
    /// </summary>
    public List<string> DisallowedPageTypes { get; set; } = new List<string>();


    public ModuleType(Type type)
    {
      ContentType = type;
    }

    public static ModuleType Convert<T>(ModuleType<T> model) where T : Module, new()
    {
      return new ModuleType(model.ContentType)
      {
        Alias = model.Alias,
        Name = model.Name,
        Description = model.Description,
        Icon = model.Icon,
        Group = model.Group,
        Tags = model.Tags,
        DisallowedPageTypes = model.DisallowedPageTypes
      };
    }
  }
}
