using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mixtape.TagHelpers;

[HtmlTargetElement(Attributes = "app-resize")]
public class ResizeTagHelper(IFileVersionProvider fileVersionProvider) : TagHelper
{
  [HtmlAttributeName("src")]
  public string Src { get; set; }

  [HtmlAttributeName("app-resize")]
  public string Preset { get; set; }

  [HtmlAttributeName("app-focal-point")]
  public string FocalPoint { get; set; }
  
  [ViewContext]
  public ViewContext ViewContext { get; set; }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    MediaFocalPoint focalPoint = null;

    if (FocalPoint != null)
    {
      string[] parts = FocalPoint.Split(',');
      focalPoint = new()
      {
        Left = decimal.Parse(parts[0].Replace('.', ',')),
        Top = decimal.Parse(parts[1].Replace('.', ','))
      };
    }

    string src = fileVersionProvider.AddFileVersionToPath(ViewContext.HttpContext.Request.PathBase, Src).Resize(Preset, focalPoint);
    output.Attributes.RemoveAll("app-resize");
    output.Attributes.RemoveAll("app-focal-point");
    output.Attributes.SetAttribute("src", src);
  }
}


[HtmlTargetElement(Attributes = Prefix + "*")]
public class ResizeCustomTagHelper(IFileVersionProvider fileVersionProvider) : TagHelper
{
  private const string Prefix = "app-resize:";

  private IDictionary<string, string> _values;

  [HtmlAttributeName("", DictionaryAttributePrefix = Prefix)]
  public IDictionary<string, string> Values
  {
    get => _values ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    set => _values = value;
  }

  [ViewContext]
  public ViewContext ViewContext { get; set; }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    foreach (KeyValuePair<string, string> value in Values)
    {
      if (output.Attributes.TryGetAttribute(value.Key, out TagHelperAttribute attr))
      {
        string parameterName = attr.Name.ToString();
        string parameterValue = attr.Value.ToString();
        string newValue = fileVersionProvider.AddFileVersionToPath(ViewContext.HttpContext.Request.PathBase, parameterValue).Resize(value.Value);
        output.Attributes.SetAttribute(parameterName, newValue);
      }
    }
  }
}