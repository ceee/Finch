using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace zero.TagHelpers;

[HtmlTargetElement(Attributes = "app-resize")]
public class ResizeTagHelper : TagHelper
{
  [HtmlAttributeName("src")]
  public string Src { get; set; }

  [HtmlAttributeName("app-resize")]
  public string Preset { get; set; }
  
  [ViewContext]
  public ViewContext ViewContext { get; set; }
  
  protected  IFileVersionProvider FileVersionProvider { get; set; }


  public ResizeTagHelper(IFileVersionProvider fileVersionProvider)
  {
    FileVersionProvider = fileVersionProvider;
  }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    string src = FileVersionProvider.AddFileVersionToPath(ViewContext.HttpContext.Request.PathBase, Src).Resize(Preset);
    output.Attributes.RemoveAll("app-resize");
    output.Attributes.SetAttribute("src", src);
  }
}


[HtmlTargetElement(Attributes = PREFIX + "*")]
public class ResizeCustomTagHelper : TagHelper
{
  private const string PREFIX = "app-resize:";

  private IDictionary<string, string> _values;

  [HtmlAttributeName("", DictionaryAttributePrefix = PREFIX)]
  public IDictionary<string, string> Values
  {
    get => _values ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    set => _values = value;
  }

  [ViewContext]
  public ViewContext ViewContext { get; set; }

  protected IFileVersionProvider FileVersionProvider { get; set; }


  public ResizeCustomTagHelper(IFileVersionProvider fileVersionProvider)
  {
    FileVersionProvider = fileVersionProvider;
  }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    foreach (var value in Values)
    {
      if (output.Attributes.TryGetAttribute(value.Key, out TagHelperAttribute attr))
      {
        string parameterName = attr.Name.ToString();
        string parameterValue = attr.Value.ToString();
        string newValue = FileVersionProvider.AddFileVersionToPath(ViewContext.HttpContext.Request.PathBase, parameterValue).Resize(value.Value);
        output.Attributes.SetAttribute(parameterName, newValue);
      }
    }
  }
}