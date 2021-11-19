//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;
//using zero.Core.Collections;
//using zero.Core.Database;
//using zero.Core.Entities;

//namespace zero.Core.Blueprints
//{
//  public class BlueprintChildInterceptor : CollectionInterceptor
//  {
//    protected IZeroContext Context { get; set; }

//    protected IZeroStore Store { get; set; }

//    protected ILogger<BlueprintChildInterceptor> Logger { get; set; }

//    protected IBlueprintService BlueprintService { get; set; }


//    public BlueprintChildInterceptor(IZeroContext context, IZeroStore store, ILogger<BlueprintChildInterceptor> logger, IBlueprintService blueprintService)
//    {
//      Context = context;
//      Store = store;
//      Logger = logger;
//      BlueprintService = blueprintService;
//    }

//    /// <inheritdoc />
//    public override bool CanRun(InterceptorParameters args, ZeroIdEntity model)
//    {
//      // this interceptor does only work for child entities
//      return args.Context.Store.ResolvedDatabase != Context.Options.Raven.Database;
//    }


//    /// <inheritdoc />
//    public override Task<InterceptorResult<ZeroIdEntity>> Deleting(InterceptorParameters args, ZeroIdEntity model)
//    {
//      if (model is not ZeroEntity || (model as ZeroEntity).Blueprint != null)
//      {
//        InterceptorResult<ZeroIdEntity> result = new();
//        result.Result = EntityResult<ZeroIdEntity>.Fail("@blueprint.errors.cannotDeleteChild");
//        return Task.FromResult(result);
//      }

//      return base.Deleting(args, model);
//    }
//  }
//}
