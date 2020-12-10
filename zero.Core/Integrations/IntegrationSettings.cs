using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Integrations
{
  public abstract class IntegrationSettings : ZeroEntity, IIntegrationSettings
  {
    public IntegrationSettings()
    {
      IsActive = true;
    }

    /// <inheritdoc />
    public string IntegrationAlias { get; set; }
  }


  [Collection("Integrations")]
  public interface IIntegrationSettings : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Preferred countries are displayed on top in lists
    /// </summary>
    string IntegrationAlias { get; set; }
  }
}
