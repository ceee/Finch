using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Text.Json;
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
  IBackofficeAssetFileSystem AssetFileSystem { get; set; }

  static string MetaInfoJson { get; set; }

  public ZeroIndexController(IZeroOptions options, IIconService iconRepository, IWebHostEnvironment env, IBackofficeAssetFileSystem assetFileSystem)
  {
    Options = options;
    IconRepository = iconRepository;
    Env = env;
    AssetFileSystem = assetFileSystem;
  }


  public async Task<ActionResult> Index()
  {
    if (Options.Version.IsNullOrEmpty())
    {
      return RedirectToAction("ZeroBackoffice", "Setup");
    }



    //return View("~/Views/Zero/Index.cshtml", new ZeroBackofficeModel()
    //{
    //  Meta = GetMetaInfo()
    //});

    BackofficeOptions options = Options.For<BackofficeOptions>();

    int port = options.DevServer.Port;
    string domain = "http://localhost:" + port;

    MetaInfoJson ??= JsonSerializer.Serialize(GetMetaInfo());

    Dictionary<string, string> model = new()
    {
      { "top", String.Empty },
      { "bottom", String.Join(String.Empty, GetJsModules(domain, new[] { "/vite/client", "/app/app.ts" })) },
      { "svg", await IconRepository.GetCompiledSvg() },
      { "meta", MetaInfoJson }
    };

    if (!Env.IsDevelopment())
    {
      string html = TokenReplacement.Apply(System.IO.File.ReadAllText(Path.Combine(Env.WebRootPath, "zero/index.html")), model);
      return Content(html, "text/html");
      //string assetHash = options.AssetHash;
      //string suffix = assetHash.HasValue() ? "?v=" + assetHash : String.Empty;

      //model["bottom"] = String.Empty;
      //model["top"] = @$"
      //  <script type='module' crossorigin src='/zero/index.js${suffix}'></script>
      //  <link rel='modulepreload' href='/zero/vendor.js${suffix}' />
      //  <link rel='stylesheet' href='/zero/index.css${suffix}' />
      //";
    }

    string content = TokenReplacement.Apply(await LoadTemplate("zero.Backoffice.Resources.backoffice.tpl.html"), model);
    return Content(content, "text/html");
  }


  ZeroBackofficeMetaInfo GetMetaInfo()
  {
    Assembly assembly = Assembly.GetEntryAssembly();
    FileInfo assemblyFileInfo = new(assembly.Location);

    return new()
    {
      AppVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
      ZeroVersion = Options.Version,
      AppLastModifiedDate = assemblyFileInfo.LastWriteTime
    };
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