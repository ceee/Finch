using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Finch.TagHelpers;

[HtmlTargetElement(Attributes = "app-if")]
public class IfTagHelper : TagHelper
{
  public bool AppIf { get; set; }

  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    if (!AppIf)
    {
      output.SuppressOutput();
    }
  }
}
