using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Finch.Frontend;

[HtmlTargetElement("app-vitescript", Attributes = "src", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ViteScriptTagHelper(IWebHostEnvironment env, IFinchOptions options) : TagHelper
{
  [HtmlAttributeNotBound]
  [ViewContext]
  public ViewContext ViewContext { get; set; } = null!;

  [HtmlAttributeName("src")]
  public string Src { get; set; }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    if (!env.IsDevelopment())
    {
      output.SuppressOutput();
      return;
    }

    int? viteProxyPort = 5123;
#if DEBUG
    viteProxyPort = options.For<ViteProxy.ViteProxyOptions>().Port;
#endif

    HttpRequest request = ViewContext.HttpContext.Request;
    string fullPath = $"{request.Scheme}://{request.Host.Host}:{viteProxyPort}/{Src}";

    output.TagName = "script";
    output.Attributes.SetAttribute("type", "module");
    output.Attributes.SetAttribute("defer", string.Empty);
    output.Attributes.SetAttribute("crossorigin", string.Empty);
    output.Attributes.SetAttribute("src", fullPath);
  }
}