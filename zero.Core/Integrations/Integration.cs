using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Integrations
{
  public abstract class Integration : ZeroEntity, IIntegration
  {
    public Integration()
    {
      IsActive = true;
    }

    /// <inheritdoc />
    public string IntegrationAlias { get; set; }
  }


  [Collection("Integrations")]
  public interface IIntegration : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Preferred countries are displayed on top in lists
    /// </summary>
    string IntegrationAlias { get; set; }
  }
}
