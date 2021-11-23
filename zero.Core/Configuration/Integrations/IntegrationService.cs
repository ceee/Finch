using Microsoft.Extensions.Logging;

namespace zero.Configuration;

public class IntegrationService : IntegrationsCollection, IIntegrationService
{
  public IntegrationService(IStoreContext<Integration> context, ILogger<IntegrationsCollection> logger) : base(context, logger)
  {
    Options = new(false);
  }
}


public interface IIntegrationService : IIntegrationsCollection { }