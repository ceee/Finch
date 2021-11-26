using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace zero.Architecture;

public class BlueprintInterceptor : Interceptor<ZeroEntity>, IBlueprintInterceptor
{
  protected IZeroContext Context { get; set; }

  protected IZeroStore Store { get; set; }

  protected ILogger<BlueprintInterceptor> Logger { get; set; }

  protected IBlueprintService BlueprintService { get; set; }

  protected IInterceptors Interceptors { get; set; }

  IList<Application> apps;

  string configuredZeroDatabase;


  public BlueprintInterceptor(IZeroContext context, IZeroStore store, ILogger<BlueprintInterceptor> logger, IBlueprintService blueprintService, IInterceptors interceptors)
  {
    Gravity = -1;
    Context = context;
    Store = store;
    Logger = logger;
    BlueprintService = blueprintService;
    configuredZeroDatabase = context.Options.For<RavenOptions>().Database;
    Interceptors = interceptors;
  }

  /// <summary>
  /// Only run when operations are on the zero database
  /// </summary>
  public override bool CanHandle(InterceptorParameters args, Type modelType) => args.Context.Store.ResolvedDatabase == configuredZeroDatabase;


  /// <inheritdoc />
  public override Task Created(InterceptorParameters args, ZeroEntity model) => Saved(args, model, false);


  /// <inheritdoc />
  public override Task Updated(InterceptorParameters args, ZeroEntity model) => Saved(args, model, true);


  /// <inheritdoc />
  public async Task Saved(InterceptorParameters args, ZeroEntity model, bool update = false)
  {
    if (!BlueprintService.TryGetBlueprint(model, out Blueprint blueprint))
    {
      return;
    }

    int count = 0;

    foreach (Application app in await GetApplications())
    {
      using ZeroContextScope scope = Context.CreateScope(app);
      IZeroDocumentSession session = scope.Store.Session(scope.Database);

      ZeroEntity child = await session.LoadAsync<ZeroEntity>(model.Id);

      if (child == null)
      {
        child = ObjectCopycat.Clone(model);
        child.Blueprint = new() { Id = model.Id };
      }
      else
      {
        blueprint.Apply(model, child);
      }

      count += 1;

      InterceptorInstruction<ZeroEntity> interceptor = update ? Interceptors.ForUpdate(model) : Interceptors.ForCreate(model);
      interceptor.Filter(x => x is not IBlueprintInterceptor);

      await interceptor.Start();
      await session.StoreAsync(child);
      await interceptor.Complete();
      await session.SaveChangesAsync();
    }

    Logger.LogDebug("Blueprint: Synced {count} children for {name} ({id})", count, model.Name, model.Id);
  }


  /// <inheritdoc />
  public override async Task Deleted(InterceptorParameters args, ZeroEntity model)
  {
    if (!BlueprintService.TryGetBlueprint(model, out Blueprint blueprint))
    {
      return;
    }

    int count = 0;

    foreach (Application app in await GetApplications())
    {
      using ZeroContextScope scope = Context.CreateScope(app);
      IZeroDocumentSession session = scope.Store.Session(scope.Database);

      count += 1;

      InterceptorInstruction<ZeroEntity> interceptor = Interceptors.ForDelete(model);
      interceptor.Filter(x => x is not IBlueprintInterceptor);

      await interceptor.Start();
      session.Delete(model.Id);      
      await interceptor.Complete();
      await session.SaveChangesAsync();
    }

    Logger.LogDebug("Blueprint: Deleted {count} children for {name} ({id})", count, model.Name, model.Id);
  }


  /// <summary>
  /// Get all applications to choose from
  /// </summary>
  async Task<IList<Application>> GetApplications()
  {
    if (apps != null)
    {
      return apps;
    }

    IAsyncDocumentSession session = Store.Session(global: true);
    apps = await session.Query<Application>().ToListAsync();
    return apps;
  }
}