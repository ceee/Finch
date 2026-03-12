using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace zero.TagHelpers;

[HtmlTargetElement(Attributes = "app-active")]
public class ActiveTagHelper(IHttpContextAccessor contextAccessor) : TagHelper
{
  [HtmlAttributeName("href")]
  public string Href { get; set; }

  public override int Order { get; } = 100;


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    output.Attributes.RemoveAll("app-active");

    if (contextAccessor.HttpContext.IsPartOfUrl(Href))
    {
      output.AddClass("is-active", HtmlEncoder.Default);
    }
    if (contextAccessor.HttpContext.IsUrl(Href))
    {
      output.AddClass("is-active-exact", HtmlEncoder.Default);
    }
  }
}


[HtmlTargetElement(Attributes = "app-active-exact")]
public class ActiveExactTagHelper(IHttpContextAccessor contextAccessor) : TagHelper
{
  [HtmlAttributeName("href")]
  public string Href { get; set; }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    output.Attributes.RemoveAll("app-active-exact");

    if (contextAccessor.HttpContext.IsUrl(Href))
    {
      output.AddClass("is-active-exact", HtmlEncoder.Default);
    }
  }
}