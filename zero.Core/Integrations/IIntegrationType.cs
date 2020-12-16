using Microsoft.Extensions.Options;
using System;

namespace zero.Core.Integrations
{
  /// <summary>
  /// An integration is an application part which has a public configuration per app.
  /// It's up to the user to provide functionality.
  /// </summary>
  public interface IIntegrationType
  {
    /// <summary>
    /// The alias
    /// </summary>
    string Alias { get; }

    /// <summary>
    /// The name of the integration (either a string or a translation key with @ prefix)
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Optional description
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Image of the integration
    /// </summary>
    string ImagePath { get; }

    /// <summary>
    /// HEX color (#aabbcc or #abc)
    /// </summary>
    string Color { get; }

    /// <summary>
    /// Type of the settings
    /// </summary>
    Type SettingsType { get; }

    /// <summary>
    /// Allow multiple instances of this configuration
    /// </summary>
    bool AllowMultiple { get; set; }

    /// <summary>
    /// Do never return null when it has not been configured yet but the default instance for a settings object
    /// </summary>
    bool IsAutoActivated { get; set; }
  }
}
