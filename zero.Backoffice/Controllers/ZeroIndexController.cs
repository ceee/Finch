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
  IIconService IconRepository { get; set; }

  public ZeroIndexController(IZeroVue zeroVue, IZeroOptions options, IAuthenticationService authService, IIconService iconRepository)
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
    int port = Options.For<BackofficeOptions>().DevServer.Port;
    string domain = "http://localhost:" + port;

    string content = TokenReplacement.Apply(await LoadTemplate("zero.Backoffice.Resources.backoffice.tpl.html"), new()
    {
      { "css", String.Empty },
      { "js", String.Join(String.Empty, GetJsModules(domain, new[] { "/vite/client", "/app/app.ts" })) },
      { "svg", await IconRepository.GetCompiledSvg() },
      { "config", config }
    });

    return Content(content, "text/html");
    //if (AuthService.IsLoggedIn())
    //{
    //  return await RenderBackoffice(domain, config);
    //}
    //else
    //{
    //  return await RenderAuth(domain, config);
    //}
  }


  async Task<ActionResult> RenderAuth(string domain, string config)
  {
    string content = TokenReplacement.Apply(await LoadTemplate("zero.Backoffice.Resources.auth.tpl.html"), new()
    {
      { "css", String.Empty },
      { "js", String.Join(String.Empty, GetJsModules(domain, new[] { "/vite/client", "/app/app-auth.js" })) },
      { "svg", await IconRepository.GetCompiledSvg() },
      { "config", config }
    });

    return Content(content, "text/html");
  }


  async Task<ActionResult> RenderBackoffice(string domain, string config)
  {
    string content = TokenReplacement.Apply(await LoadTemplate("zero.Backoffice.Resources.backoffice.tpl.html"), new()
    {
      { "css", String.Empty },
      { "js", String.Join(String.Empty, GetJsModules(domain, new[] { "/vite/client", "/app/app.js" })) },
      { "svg", await IconRepository.GetCompiledSvg() },
      { "config", config }
    });

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

public class ZeroBackofficeModel
{
  public int Port { get; set; }

  public IZeroVue Vue { get; set; }
}
