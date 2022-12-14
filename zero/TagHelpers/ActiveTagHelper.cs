using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace zero.TagHelpers;

[HtmlTargetElement(Attributes = "app-active")]
public class ActiveTagHelper : TagHelper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  [HtmlAttributeName("class")]
  public string Classes { get; set; }

  public override int Order => 100;


  public ActiveTagHelper(IHttpContextAccessor contextAccessor)
  {
    _httpContextAccessor = contextAccessor;
  }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    output.Attributes.RemoveAll("app-active");

    output.Attributes.TryGetAttribute("Href", out TagHelperAttribute _href);
    string href = _href?.Value?.ToString() ?? string.Empty;

    HashSet<string> classes = Classes?.Split(" ").ToHashSet() ?? new HashSet<string>();

    if (_httpContextAccessor.HttpContext.IsPartOfUrl(href))
    {
      classes.Add("is-active");
    }
    if (_httpContextAccessor.HttpContext.IsUrl(href))
    {
      classes.Add("is-active-exact");
    }

    output.Attributes.SetAttribute("class", string.Join(" ", classes));
  }
}