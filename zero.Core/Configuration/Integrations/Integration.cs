using System.Text.Json.Serialization;

namespace zero.Configuration;

/// <summary>
/// An integration is an application part which has a public configuration per app.
/// It's up to the user to provide functionality.
/// </summary>
[RavenCollection("Integrations")]
public class Integration : ZeroEntity
{
  /// <inheritdoc />
  public string TypeAlias { get; set; }
}