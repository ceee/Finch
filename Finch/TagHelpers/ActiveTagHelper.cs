using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Finch.TagHelpers;

[HtmlTargetElement(Attributes = "app-active")]
public class ActiveTagHelper(IHttpContextAccessor contextAccessor) : TagHelper
{
  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    string href = output.Attributes["href"]?.Value?.ToString();

    output.Attributes.RemoveAll("app-active");

    if (href == null)
    {
      return;
    }
    if (contextAccessor.HttpContext.IsPartOfUrl(href))
    {
      output.AddClass("is-active", HtmlEncoder.Default);
    }
    if (contextAccessor.HttpContext.IsUrl(href))
    {
      output.AddClass("is-active-exact", HtmlEncoder.Default);
    }
  }
}


[HtmlTargetElement(Attributes = "app-active-exact")]
public class ActiveExactTagHelper(IHttpContextAccessor contextAccessor) : TagHelper
{
  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    string href = output.Attributes["href"]?.Value?.ToString();

    output.Attributes.RemoveAll("app-active-exact");

    if (href == null)
    {
      return;
    }
    if (contextAccessor.HttpContext.IsUrl(href))
    {
      output.AddClass("is-active-exact", HtmlEncoder.Default);
    }
  }
}