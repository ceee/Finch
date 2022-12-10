using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace zero.TagHelpers;

[HtmlTargetElement(Attributes = "zero-resize")]
public class ResizeTagHelper : TagHelper
{
  [HtmlAttributeName("src")]
  public string Src { get; set; }

  [HtmlAttributeName("zero-resize")]
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
    output.Attributes.RemoveAll("zero-resize");
    output.Attributes.SetAttribute("src", src);
  }
}