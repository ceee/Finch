using zero.Core.Entities;
using zero.Core.Integrations;

namespace zero.Core.Options
{
  public class IntegrationOptions : ZeroBackofficeCollection<IIntegrationType>, IZeroCollectionOptions
  {
    public void Add<T>() where T : IIntegrationType, new()
    {
      Items.Add(new T());
    }
  }
}
