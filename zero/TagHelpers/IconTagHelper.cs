using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace zero.TagHelpers;

[HtmlTargetElement("app-icon", TagStructure = TagStructure.NormalOrSelfClosing)]
public class IconTagHelper : TagHelper
{
  public string Symbol { get; set; }

  public int Size { get; set; } = -1;

  public decimal Stroke { get; set; } = -1;

  public string Class { get; set; }

  [HtmlAttributeNotBound]
  [ViewContext]
  public ViewContext ViewContext { get; set; } = default!;


  private IconOptions _options;

  private IFileVersionProvider _fileVersionProvider;

  private readonly ILogger<IconTagHelper> _logger;

  private readonly IFileVersionProvider _fileVersionProvider;

  [HtmlAttributeNotBound]
  [ViewContext]
  public ViewContext ViewContext { get; set; } = default;


  public IconTagHelper(IOptionsMonitor<IconOptions> options, ILogger<IconTagHelper> logger, IFileVersionProvider fileVersionProvider)
  {
    _options = options.CurrentValue;
    _logger = logger;
    _fileVersionProvider = fileVersionProvider;
    options.OnChange(val => _options = val);
  }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    if (_options.Path.IsNullOrWhiteSpace())
    {
      _logger.LogWarning("Could not render <app-icon />. Please configure the path with IconOptions before.");
      output.SuppressOutput();
      return;
    }
    
    int size = Size < 0 ? _options.DefaultSize : Size;
    string stroke = (Stroke < 0 ? _options.DefaultStrokeWidth : Stroke).ToString().Replace(',', '.');

    output.TagName = "svg";
    output.Attributes.SetAttribute("class", Class.HasValue() ? $"{_options.CssClass} {Class}" : _options.CssClass);
    output.Attributes.SetAttribute("width", size);
    output.Attributes.SetAttribute("height", size);
    output.Attributes.SetAttribute("xmlns", "http://www.w3.org/2000/svg");
    output.Attributes.SetAttribute("stroke-width", stroke);
    output.Attributes.SetAttribute("data-symbol", Symbol);
    output.Content.SetHtmlContent(BuildIcon(Symbol, size, stroke));
  }


  public string BuildIcon(string symbol, int size = 18, string stroke = "2", string classes = null, bool withSvg = false)
  {
    string path = _fileVersionProvider.AddFileVersionToPath(ViewContext.HttpContext.Request.PathBase, _options.Path);
    string inner = $"<use xlink:href='{path}#{symbol}\'></use>";

    if (!withSvg)
    {
      return inner;
    }

    return $"<svg class='{_options.CssClass}{(classes.HasValue() ? " " + classes : string.Empty)}' width='{size}' height='{size}'" +
      $"xmlns='http://www.w3.org/2000/svg' stroke-width='{stroke}' data-symbol='{symbol}'>{inner}</svg>";
  }
}