using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Blueprints
{
  public class BlueprintChildInterceptor : CollectionInterceptor
  {
    protected IZeroContext Context { get; set; }

    protected IZeroStore Store { get; set; }

    protected ILogger<BlueprintChildInterceptor> Logger { get; set; }

    protected IBlueprintService BlueprintService { get; set; }


    public BlueprintChildInterceptor(IZeroContext context, IZeroStore store, ILogger<BlueprintChildInterceptor> logger, IBlueprintService blueprintService)
    {
      Context = context;
      Store = store;
      Logger = logger;
      BlueprintService = blueprintService;
    }

    /// <inheritdoc />
    public override bool CanRun(Parameters args)
    {
      // this interceptor does only work for child entities
      return (args.Session as AsyncDocumentSession)?.DatabaseName != Context.Options.Raven.Database;
    }


    /// <inheritdoc />
    public override Task<InterceptorResult<ZeroEntity>> Deleting(DeleteParameters args)
    {
      if (args.Model.Blueprint != null)
      {
        InterceptorResult<ZeroEntity> result = new();
        result.Result = EntityResult<ZeroEntity>.Fail("@blueprint.errors.cannotDeleteChild");
        return Task.FromResult(result);
      }

      return base.Deleting(args);
    }
  }
}
