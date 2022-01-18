using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;
using zero.Api.Controllers;
using zero.Backoffice.Services;

namespace zero.Backoffice.Controllers;

[ZeroAuthorize(false)]
[DisableBrowserCacheFilter]
public class ZeroIndexController : Controller
{
  IZeroOptions Options { get; set; }
  IIconService IconRepository { get; set; }
  IWebHostEnvironment Env { get; set; }

  public ZeroIndexController(IZeroOptions options, IIconService iconRepository, IWebHostEnvironment env)
  {
    Options = options;
    IconRepository = iconRepository;
    Env = env;
  }


  public async Task<ActionResult> Index()
  {
    if (Options.Version.IsNullOrEmpty())
    {
      return RedirectToAction("ZeroBackoffice", "Setup");
    }

    int port = Options.For<BackofficeOptions>().DevServer.Port;
    string domain = "http://localhost:" + port;

    Dictionary<string, string> model = new()
    {
      { "top", String.Empty },
      { "bottom", String.Join(String.Empty, GetJsModules(domain, new[] { "/vite/client", "/app/app.ts" })) },
      { "svg", await IconRepository.GetCompiledSvg() }
    };

    if (!Env.IsDevelopment() || true)
    {
      model["bottom"] = String.Empty;
      model["top"] = @"
        <script type='module' crossorigin src='/zero/index.js'></script>
        <link rel='modulepreload' href='/zero/vendor.js' />
        <link rel='stylesheet' href='/zero/index.css' />
      ";
    }

    string content = TokenReplacement.Apply(await LoadTemplate("zero.Backoffice.Resources.backoffice.tpl.html"), model);
    return Content(content, "text/html");
  }


  IEnumerable<string> GetJsModules(string domain, string[] modules)
  {
    return modules.Select(path => $"<script type='module' src='{domain.TrimEnd('/')}{path.EnsureStartsWith('/')}'></script>");
  }


  async Task<string> LoadTemplate(string resourceName)
  {
    using Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName);
    using StreamReader reader = new(stream);
    return await reader.ReadToEndAsync();
  }
}