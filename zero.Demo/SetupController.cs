using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Indexes;
using System.Text.Json;
using zero.Configuration;
using zero.Context;
using zero.Persistence;
using zero.Routing;
using zero.Utils;

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


  [HttpGet("/api/setup/json")]
  public async Task<IActionResult> TestJson([FromServices] IIntegrationStore store)
  {
    Integration integration = await store.Load("analytics.fathom");

    JsonFlavorVariantConverter converter = new();
    JsonSerializerOptions opts = new();
    opts.Converters.Add(converter);

    return Content(JsonSerializer.Serialize(integration, opts), "application/json");
  }


  [HttpGet("/api/setup/indexes")]
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


  [HttpGet("/api/setup/routes")]
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