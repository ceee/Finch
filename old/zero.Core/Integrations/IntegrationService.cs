using Microsoft.Extensions.Logging;
using zero.Core.Collections;

namespace zero.Core.Integrations
{
  public class IntegrationService : IntegrationsCollection, IIntegrationService
  {
    public IntegrationService(ICollectionContext<Integration> context, ILogger<IntegrationsCollection> logger) : base(context, logger)
    {
      Options = new(false);
    }
  }


  public interface IIntegrationService : IIntegrationsCollection { }
}
