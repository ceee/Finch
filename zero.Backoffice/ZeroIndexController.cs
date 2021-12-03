using Microsoft.AspNetCore.Mvc;
using System.IO;
using zero.Api.Controllers;

namespace zero.Backoffice.Controllers;

[ZeroAuthorize(false)]
[DisableBrowserCache]
public class ZeroIndexController : Controller
{
  IZeroVue ZeroVue { get; set; }
  IZeroOptions Options { get; set; }

  string[] JsDevModulePaths = new[] { "/vite/client", "/app/app.js" };

  public ZeroIndexController(IZeroVue zeroVue, IZeroOptions options)
  {
    ZeroVue = zeroVue;
    Options = options;
  }


  public async Task<ActionResult> Index()
  {
    if (Options.Version.IsNullOrEmpty())
    {
      return RedirectToAction("ZeroBackoffice", "Setup");
    }

    string config = await ZeroVue.ConfigAsJson();

    int port = Options.For<BackofficeOptions>().DevServer.Port;

    string resourceName = $"zero.Backoffice.Resources.index.tpl.html";

    using Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName);
    using StreamReader reader = new(stream);

    string template = await reader.ReadToEndAsync();

    string content = TokenReplacement.Apply(template, new()
    {
      { "css", String.Empty },
      { "js", String.Join(String.Empty, GetJsModules("http://localhost:" + port)) },
      { "svg", ZeroVue.GetIconSvg() },
      { "config", config }
    });

    return Content(content, "text/html");
  }


  IEnumerable<string> GetJsModules(string domain)
  {
    return JsDevModulePaths.Select(path => $"<script type='module' src='{domain.TrimEnd('/')}{path.EnsureStartsWith('/')}'></script>");
  }
}

public class ZeroBackofficeModel
{
  public int Port { get; set; }

  public IZeroVue Vue { get; set; }
}
