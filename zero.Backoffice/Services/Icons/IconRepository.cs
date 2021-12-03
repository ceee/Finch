using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace zero.Backoffice.Services;

public class IconRepository : IIconRepository
{
  protected IOptions<BackofficeOptions> Options { get; set; }

  protected IBackofficeAssetFileSystem FileSystem { get; set; }

  protected ILogger<IconRepository> Logger { get; set; }

  protected ConcurrentBag<BackofficeIconSetPresentation> CachedSets { get; private set; } = new();

  protected string CachedSvg { get; set; }

  protected bool IsLoaded { get; set; }


  public IconRepository(IOptions<BackofficeOptions> options, IBackofficeAssetFileSystem fileSystem, ILogger<IconRepository> logger)
  {
    Options = options;
    FileSystem = fileSystem;
    Logger = logger;
  }


  /// <inheritdoc />
  public async Task<IEnumerable<BackofficeIconSetPresentation>> GetSets()
  {
    await EnsureIconsAreLoaded();
    return CachedSets;
  }


  /// <inheritdoc />
  public async Task<string> GetCompiledSvg()
  {
    await EnsureIconsAreLoaded();
    return CachedSvg;
  }


  async Task EnsureIconsAreLoaded()
  {
    if (IsLoaded)
    {
      return;
    }

    StringBuilder svg = new();

    foreach (BackofficeIconSet set in Options.Value.IconSets)
    {
      if (!await FileSystem.Exists(set.SpritePath))
      {
        Logger.LogWarning("Could not load icon set {alias} from path {path}", set.Alias, FileSystem.Map(set.SpritePath));
        continue;
      }

      using Stream stream = await FileSystem.StreamFile(set.SpritePath);

      XDocument xml = XDocument.Load(stream);
      IEnumerable<XElement> symbols = xml.Descendants().Where(x => x.Name.LocalName == "symbol");

      if (!symbols.Any())
      {
        Logger.LogWarning("Icon set {alias} does not contain any <symbol>", set.Alias);
        continue;
      }

      HashSet<string> icons = new();

      foreach (XElement symbol in symbols)
      {
        string symbolAlias = set.Prefix + "-" + symbol.Attribute("id").Value.ToString();
        symbol.SetAttributeValue("id", symbolAlias);
        svg.Append(symbol.ToString().RemoveNewLines());
        icons.Add(symbolAlias);
      }

      CachedSets.Add(new()
      {
        Alias = set.Alias,
        Name = set.Name,
        Prefix = set.Prefix,
        Icons = icons
      });
    }

    CachedSvg = svg.ToString();
    IsLoaded = true;
  }
}


public interface IIconRepository
{
  /// <summary>
  /// Get all registered icon sets with all their containing icons
  /// </summary>
  Task<IEnumerable<BackofficeIconSetPresentation>> GetSets();

  /// <summary>
  /// Get compiled SVG string for alle registered icon sets
  /// </summary>
  Task<string> GetCompiledSvg();
}