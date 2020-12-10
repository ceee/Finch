using System;

namespace zero.Core.Integrations
{
  /// <summary>
  /// An integration is an application part which has a public configuration per app.
  /// It's up to the user to provide functionality.
  /// </summary>
  public class Integration : IIntegration
  {
    /// <inheritdoc />
    public string Alias { get; set; }

    /// <inheritdoc />
    public string Name { get; set; }

    /// <inheritdoc />
    public string Description { get; set; }

    /// <inheritdoc />
    public string ImagePath { get; set; }

    /// <inheritdoc />
    public string Color { get; set; }

    /// <inheritdoc />
    public Type SettingsType { get; set; }

    /// <inheritdoc />
    public bool AllowMultiple { get; set; }

    /// <inheritdoc />
    public bool IsAutoActivated { get; set; }
  }
}
