using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using System.Text.Json;
using zero.Configuration;
using zero.Context;
using zero.Localization;
using zero.Persistence;
using zero.Routing;
using zero.Stores;
using zero.Utils;

namespace zero.Demo.Controllers;

[ApiController]
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
  public async Task<IActionResult> TestJson([FromServices] IZeroStore store, [FromServices] IZeroOptions options)
  {
    using var session = store.Session();
    var items = await session.Query<Integration>().ToListAsync();

    JsonSerializerOptions opts = new();
    opts.Converters.Add(new JsonFlavorVariantConverterFactory(options));

    var json = JsonSerializer.Serialize(items, opts);
    return Content(json, "application/json");
  }


  [HttpGet("/api/setup/json1")]
  public async Task<IActionResult> TestJson1([FromServices] IZeroStore store, [FromServices] IZeroOptions options)
  {
    using var session = store.Session();
    var items = await session.Query<Integration>().ToListAsync();

    JsonSerializerOptions opts = new();
    opts.Converters.Add(new JsonFlavorVariantConverterFactory(options));

    return Json(items, opts);
  }


  //[HttpGet("/api/setup/json")]
  //public async Task<IActionResult> TestJson([FromServices] ICountryStore store)
  //{
  //  var countries = await store.Load(pageNumber: 1, pageSize: 10);
  //  var json = JsonSerializer.Serialize(countries);
  //  return Json(json, "application/json");
  //}


  [HttpPost("/api/setup/postjson")]
  public IActionResult PostTestJson(Country country)
  {
    return Json(new { type = country.GetType().FullName, model = country });
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