using Microsoft.Extensions.Logging;
using zero.Core.Collections;
using zero.Core.Options;

namespace zero.Core.Integrations
{
  public class IntegrationService : IntegrationsCollection, IIntegrationService
  {
    public IntegrationService(IZeroOptions options, IZeroContext context, ILogger<IntegrationsCollection> logger, ICollectionInterceptorHandler interceptorHandler = null) : base(context, options, logger, interceptorHandler) { }
  }


  public interface IIntegrationService : IIntegrationsCollection { }
}
