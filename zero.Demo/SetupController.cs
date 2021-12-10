using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Indexes;
using zero.Context;
using zero.Persistence;
using zero.Routing;

namespace zero.Demo.Controllers;

public class SetupController : Controller
{
  //ISetupApi Api;

  //public SetupController(ISetupApi api)
  //{
  //  Api = api;
  //}


  //[HttpGet]
  //public async Task<IActionResult> Install([FromQuery] string password)
  //{
  //  var result = await Api.Install(new SetupModel()
  //  {
  //    AppName = "switchcase",
  //    ContentRootPath = "O:/zero/zero.Web",
  //    Database = new SetupDatabaseModel()
  //    {
  //      Name = "switchcase",
  //      Url = "http://localhost:9800"
  //    },
  //    User = new SetupUserModel()
  //    {
  //      Email = "cee@live.at",
  //      Name = "Tobias Klika",
  //      Password = password
  //    }
  //  });

  //  return Json(result);
  //}


  [HttpGet]
  public async Task<IActionResult> Indexes([FromServices] IZeroStore store, [FromServices] IZeroContext context)
  {
    var indexes = context.Options.For<RavenOptions>().Indexes;
    var builtIndexes = indexes.BuildAll(context.Options, store.Raven);

    await IndexCreation.CreateIndexesAsync(builtIndexes, store.Raven, database: "laola");
    await IndexCreation.CreateIndexesAsync(builtIndexes, store.Raven, database: "laola.hofbauer");
    await IndexCreation.CreateIndexesAsync(builtIndexes, store.Raven, database: "laola.brothers");

    return Json(new
    {
      indexes = true
    });
  }


  [HttpGet]
  public async Task<IActionResult> Routes([FromServices] IZeroStore store, [FromServices] IZeroContext context, [FromServices] IEnumerable<IRouteProvider> providers)
  {
    RouteBulkRefresher refresher = new(store, context, providers);

    int routes = await refresher.RebuildAllRoutes();

    return Json(new
    {
      routes = routes
    });
  }
}