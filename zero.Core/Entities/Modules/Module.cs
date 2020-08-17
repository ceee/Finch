using System;

namespace zero.Core.Entities
{
  /// <summary>
  /// A module can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  public class Module : IModule
  {
    /// <inheritdoc />
    public string Id { get; set; }

    /// <inheritdoc />
    public uint Sort { get; set; }

    /// <inheritdoc />
    public bool IsActive { get; set; }

    /// <inheritdoc />
    public string ModuleTypeAlias { get; set; }
  }


  public interface IModule : IZeroIdEntity
  {
    /// <summary>
    /// Sort order
    /// </summary>
    uint Sort { get; set; }

    /// <summary>
    /// Whether the module is visible in the frontend
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Alias of the used module type
    /// </summary>
    string ModuleTypeAlias { get; set; }
  }
}