using zero.Core.Entities;
using zero.Core.Integrations;

namespace zero.Core.Options
{
  public class IntegrationOptions : ZeroBackofficeCollection<IIntegration>, IZeroCollectionOptions
  {
    public void Add<T>() where T : IIntegration, new()
    {
      Items.Add(new T());
    }
  }
}
