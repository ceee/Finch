using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using zero.Security;

namespace zero.TagHelpers;

[HtmlTargetElement("app-captcha", TagStructure = TagStructure.NormalOrSelfClosing)]
public class CaptchaTagHelper(IOptionsMonitor<CaptchaOptions> options) : TagHelper
{
  [HtmlAttributeNotBound]
  [ViewContext]
  public ViewContext ViewContext { get; set; } = null!;

  private readonly CaptchaOptions _options = options.CurrentValue;


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    if (!_options.Enabled)
    {
      output.SuppressOutput();
      return;
    }
    output.TagName = "cap-widget";
    output.Attributes.SetAttribute("class", "cap-widget");
    output.Attributes.SetAttribute("data-cap-api-endpoint", _options.Endpoint);
    output.Attributes.SetAttribute("data-cap-hidden-field-name", _options.HiddenFieldName);

    string wasmFilePath =_options.Endpoint + "/cap.wasm";
    string widgetFilePath =_options.Endpoint + "/cap.widget.js";
    output.PreElement.AppendHtml($"<script>window.CAP_CUSTOM_WASM_URL = '{wasmFilePath}';</script>");
    output.PreElement.AppendHtml($"<script type='module' src='{widgetFilePath}'></script>");

    foreach ((string key, string value) in _options.Localization)
    {
      output.Attributes.SetAttribute($"data-cap-i18n-{key}", value);
    }
  }
}