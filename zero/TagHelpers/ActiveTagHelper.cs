using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace zero.TagHelpers;

[HtmlTargetElement(Attributes = "app-active")]
public class ActiveTagHelper : TagHelper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  [HtmlAttributeName("class")]
  public string Classes { get; set; }

  [HtmlAttributeName("href")]
  public string Href { get; set; }

  public override int Order => 100;


  public ActiveTagHelper(IHttpContextAccessor contextAccessor)
  {
    _httpContextAccessor = contextAccessor;
  }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    output.Attributes.RemoveAll("app-active");

    HashSet<string> classes = Classes?.Split(" ").ToHashSet() ?? new HashSet<string>();

    if (_httpContextAccessor.HttpContext.IsPartOfUrl(Href))
    {
      classes.Add("is-active");
    }
    if (_httpContextAccessor.HttpContext.IsUrl(Href))
    {
      classes.Add("is-active-exact");
    }

    output.Attributes.SetAttribute("class", string.Join(" ", classes));
  }
}