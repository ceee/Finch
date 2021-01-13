using System;
using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Integrations
{
  /// <inheritdoc />
  public class Integration : ZeroEntity, IIntegration
  {
    /// <inheritdoc />
    public string TypeAlias { get; set; }
  }


  /// <summary>
  /// An integration is an application part which has a public configuration per app.
  /// It's up to the user to provide functionality.
  /// </summary>
  [Collection("Integrations")]
  public interface IIntegration : IZeroTypedEntity, IZeroDbConventions { }
}