using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Blueprints
{
  public class BlueprintInterceptor : CollectionInterceptor
  {
    protected IZeroContext Context { get; set; }

    protected IZeroStore Store { get; set; }

    protected ILogger<BlueprintInterceptor> Logger { get; set; }

    protected IBlueprintService BlueprintService { get; set; }

    IList<Application> Apps { get; set; }


    public BlueprintInterceptor(IZeroContext context, IZeroStore store, ILogger<BlueprintInterceptor> logger, IBlueprintService blueprintService)
    {
      Context = context;
      Store = store;
      Logger = logger;
      BlueprintService = blueprintService;
    }

    /// <inheritdoc />
    public override bool CanRun(Parameters args)
    {
      // we do only update children if we operate on the shared database
      return (args.Session as AsyncDocumentSession)?.DatabaseName == Context.Options.Raven.Database;
    }


    /// <inheritdoc />
    public override async Task Saved(SaveParameters args)
    {
      if (!BlueprintService.TryGetBlueprint(args.Model, out Blueprint blueprint))
      {
        return;
      }

      int count = 0;

      foreach (Application app in await GetApplications())
      {
        using ZeroContextScope scope = Context.CreateScope(app);
        IZeroDocumentSession session = scope.Store.Session(scope.Database);

        ZeroEntity child = default;

        if (args.IsUpdate)
        {
          child = await session.LoadAsync<ZeroEntity>(args.Model.Id);
        }

        if (child == null)
        {
          child = args.Model.Clone();
          child.Blueprint = new() { Id = args.Model.Id };
        }
        else
        {          
          blueprint.Apply(args.Model, child);
        }

        count += 1;

        // now we have to store the child
        // but this will not work with the session as we need to run the scoped interceptors,
        // therefore we need access to the collection
        await session.StoreAsync(child);
        await session.SaveChangesAsync();
      }

      Logger.LogDebug("Blueprint: Synced {count} children for {name} ({id})", count, args.Model.Name, args.Id);
    }


    /// <inheritdoc />
    public override async Task Deleted(DeleteParameters args)
    {
      if (!BlueprintService.TryGetBlueprint(args.Model, out Blueprint blueprint))
      {
        return;
      }

      int count = 0;

      foreach (Application app in await GetApplications())
      {
        using ZeroContextScope scope = Context.CreateScope(app);
        IZeroDocumentSession session = scope.Store.Session(scope.Database);

        count += 1;

        // now we have to delete the child
        // but this will not work with the session as we need to run the scoped interceptors,
        // therefore we need access to the collection
        session.Delete(args.Model.Id);
        await session.SaveChangesAsync();
      }

      Logger.LogDebug("Blueprint: Deleted {count} children for {name} ({id})", count, args.Model.Name, args.Id);
    }


    /// <summary>
    /// Get all applications to choose from
    /// </summary>
    async Task<IList<Application>> GetApplications()
    {
      if (Apps != null)
      {
        return Apps;
      }

      IAsyncDocumentSession session = Store.Session(global: true);
      Apps = await session.Query<Application>().ToListAsync();
      return Apps;
    }




    /// <inheritdoc />
    //public override async Task Saved(SaveParameters args)
    //{

    //  //Logger.LogInformation("Route updates completed (+{added}/~{updated}/-{removed}) for {model} (id: {id})", countRoutes - countUpdatedRoutes, countUpdatedRoutes, obsoleteRoutes.Count, args.Model.Name, args.Model.Id);
    //}
  }
}
