using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SixLabors.ImageSharp.Web.Commands;

namespace zero.Media.ImageSharp;

/// <summary>
/// Parses commands from the request querystring.
/// </summary>
public sealed class PresetRequestParser : IRequestParser
{
  MediaOptions _options;
  ILogger<PresetRequestParser> _logger;

  const char COLON = ':';
  const string PREFIX = "/:";
  const char SLASH = '/';

  const string MIME_AVIF = "image/avif";
  const string MIME_WEBP = "image/webp";


  public PresetRequestParser(ILogger<PresetRequestParser> logger, IOptionsMonitor<MediaOptions> monitor)
  {
    _logger = logger;
    _options = monitor.CurrentValue;
    monitor.OnChange(opts => _options = opts);
  }


  /// <inheritdoc/>
  public CommandCollection ParseRequestCommands(string preset, HttpContext context = null)
  {
    string focalPoint = null;

    int focalIndex = preset.IndexOf(COLON);

    if (focalIndex > -1)
    {
      focalPoint = preset.Substring(focalIndex + 1, preset.Length - focalIndex - 1);
      preset = preset.Substring(0, focalIndex);
    }

    if (preset == "{rx}")
    {
      return new();
    }

    if (!_options.ImageSharp.Presets.TryGetValue(preset, out string[] transformed))
    {
      _logger.LogWarning("Could not load imaging preset {preset}", preset);
      return new();
    }

    Dictionary<string, string> filters = transformed.Select(x => x.Split("=", 2)).ToDictionary(x => x[0], x => x[1]);

    if (focalPoint != null)
    {
      filters["rxy"] = focalPoint;
    }
    else
    {
      filters["ranchor"] = "center";
    }

    FallbackFormat fallbackFormat = FindFallbackFormatInContext(context);

    // fall back to webp, as avif is not supported yet by ImageSharp
    if (!filters.ContainsKey("format") && fallbackFormat != FallbackFormat.None)
    {
      filters["format"] = "webp"; 
    }

    if (!filters.ContainsKey("quality"))
    {
      filters["quality"] = _options.ImageSharp.DefaultQuality.ToString();
    }

    CommandCollection collection = new();

    foreach (KeyValuePair<string, string> filter in filters)
    {
      collection.Add(filter);
    }

    return collection;
  }


  /// <inheritdoc/>
  public FallbackFormat FindFallbackFormatInContext(HttpContext context)
  {
    string acceptKey = Microsoft.Net.Http.Headers.HeaderNames.Accept;

    if (context == null || context.Request == null || !context.Request.Headers.ContainsKey(acceptKey))
    {
      return FallbackFormat.None;
    }

    string acceptHeader = context.Request.Headers[acceptKey].ToString();

    if (acceptHeader.IsNullOrEmpty())
    {
      return FallbackFormat.None;
    }
    if (acceptHeader.Contains(MIME_AVIF))
    {
      return FallbackFormat.Avif;
    }
    if (acceptHeader.Contains(MIME_WEBP))
    {
      return FallbackFormat.Webp;
    }
    return FallbackFormat.None;
  }


  /// <inheritdoc/>
  public CommandCollection ParseRequestCommands(HttpContext context)
  {
    string path = context.Request.Path.Value;
    int startSlash = path.IndexOf(PREFIX);
    int lastSlash = path.LastIndexOf(SLASH);

    if (startSlash < 0)
    {
      return new();
    }

    string preset = path.Substring(startSlash + PREFIX.Length, lastSlash - startSlash - PREFIX.Length);
    return ParseRequestCommands(preset, context);
  }


  public enum FallbackFormat
  {
    None = 0,
    Webp = 1,
    Avif = 2
  }
}
