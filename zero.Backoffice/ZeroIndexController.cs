using Microsoft.AspNetCore.Mvc;
using System.IO;
using zero.Api.Controllers;
using zero.Backoffice.Services;

namespace zero.Backoffice.Controllers;

[ZeroAuthorize(false)]
[DisableBrowserCache]
public class ZeroIndexController : Controller
{
  IZeroVue ZeroVue { get; set; }
  IZeroOptions Options { get; set; }
  IAuthenticationService AuthService { get; set; }
  IIconRepository IconRepository { get; set; }

  string[] JsDevModulePaths = new[] { "/vite/client", "/app/app.js" };

  public ZeroIndexController(IZeroVue zeroVue, IZeroOptions options, IAuthenticationService authService, IIconRepository iconRepository)
  {
    ZeroVue = zeroVue;
    Options = options;
    AuthService = authService;
    IconRepository = iconRepository;
  }


  public async Task<ActionResult> Index()
  {
    if (Options.Version.IsNullOrEmpty())
    {
      return RedirectToAction("ZeroBackoffice", "Setup");
    }

    string config = await ZeroVue.ConfigAsJson();
    bool isLoggedIn = AuthService.IsLoggedIn();
    int port = Options.For<BackofficeOptions>().DevServer.Port;

    string resourceName = $"zero.Backoffice.Resources.backoffice.tpl.html";

    string template = await LoadTemplate(resourceName);

    string content = TokenReplacement.Apply(template, new()
    {
      { "css", String.Empty },
      { "js", String.Join(String.Empty, GetJsModules("http://localhost:" + port)) },
      { "svg", await IconRepository.GetCompiledSvg() },
      { "config", config }
    });

    return Content(content, "text/html");
  }


  //async Task<ActionResult> Login()
  //{
  //  string content = TokenReplacement.Apply(await LoadTemplate("zero.Backoffice.Resources.backoffice.tpl.html"), new()
  //  {
  //    { "css", String.Empty },
  //    { "js", String.Join(String.Empty, GetJsModules("http://localhost:" + port)) },
  //    { "svg", ZeroVue.GetIconSvg() },
  //    { "config", config }
  //  });
  //}


  //async Task<ActionResult> Backoffice()
  //{

  //}


  IEnumerable<string> GetJsModules(string domain)
  {
    return JsDevModulePaths.Select(path => $"<script type='module' src='{domain.TrimEnd('/')}{path.EnsureStartsWith('/')}'></script>");
  }


  async Task<string> LoadTemplate(string resourceName)
  {
    using Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName);
    using StreamReader reader = new(stream);
    return await reader.ReadToEndAsync();
  }
}

public class ZeroBackofficeModel
{
  public int Port { get; set; }

  public IZeroVue Vue { get; set; }
}
